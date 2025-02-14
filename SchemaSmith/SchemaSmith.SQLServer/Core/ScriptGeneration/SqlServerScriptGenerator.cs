using System.Text;
using SchemaSmith.Domain.Interfaces;
using SchemaSmith.SQLServer.Domain;

namespace SchemaSmith.SQLServer.Core.ScriptGeneration;

public class SqlServerScriptGenerator : ICreateScriptGenerator<Database>
{
    public string Generate(Database database)
    {
        StringBuilder sb = new();

        Obliterate(sb, database);
        CreateSchemaAndTables(database, sb);
        AddForeignKeys(database, sb);

        // TODO: config for metadata
        if (false)
            AddMetaData(database, sb);

        RunScripts(database, sb);

        return sb.ToString();
    }

    private static void RunScripts(Database database, StringBuilder sb)
    {
        sb.AppendLine(ScriptTemplates.RunScriptsHeader);

        foreach (var schema in database.Schemas)
        {
            foreach (var table in schema.Tables)
            {
                foreach (var script in table.Scripts)
                {
                    var scriptContent = File.ReadAllText(script.Source);
                    sb.AppendLine(scriptContent);
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }
            }
        }
    }

    private static void Obliterate(StringBuilder sb, Database database)
    {
        sb.AppendLine(ScriptTemplates.ObliterateText);

        foreach (var schema in database.Schemas)
            sb.AppendLine(ScriptTemplates.DropAllFKs(schema.Name));

        foreach (var schema in database.Schemas)
        {
            sb.AppendLine(ScriptTemplates.DropAllTablesAndSchema(schema.Name));
        }
    }

    private static void CreateSchemaAndTables(Database database, StringBuilder sb)
    {
        foreach (var schema in database.Schemas)
        {
            sb.AppendLine(ScriptTemplates.CreateSchemaHeader(schema.Name));

            AddSchema(sb, schema);

            foreach (var table in schema.Tables)
            {
                AppendTable(sb, schema, table);
            }
        }
    }

    private static void AddSchema(StringBuilder sb, Domain.Schema schema)
    {
        sb.AppendLine($"EXECUTE('CREATE SCHEMA [{schema.Name}] AUTHORIZATION [dbo]')");
        sb.AppendLine($"GO");
        sb.AppendLine();
    }

    private static void AppendTable(
        StringBuilder sb,
        Domain.Schema schema,
        Table table)
    {
        DropTableIfExists(sb, schema, table);
        CreateTable(sb, schema, table);
        AddPrimaryKey(sb, schema, table);
        AddIndexes(sb, schema, table);
        AddColumnstoreIndexes(sb, schema, table);
    }

    private static void DropTableIfExists(StringBuilder sb, Domain.Schema schema, Table table)
    {
        sb.AppendLine($"IF EXISTS (");
        sb.AppendLine($"    SELECT *");
        sb.AppendLine($"    FROM INFORMATION_SCHEMA.TABLES");
        sb.AppendLine($"    WHERE TABLE_SCHEMA = '{schema.Name}' AND TABLE_NAME = '{table.Name}'");
        sb.AppendLine($")");
        sb.AppendLine($"    DROP TABLE [{schema.Name}].[{table.Name}]");
        sb.AppendLine("GO");
        sb.AppendLine();
    }

    private static void CreateTable(StringBuilder sb, Domain.Schema schema, Table table)
    {
        sb.AppendLine($"CREATE TABLE [{schema.Name}].[{table.Name}]");
        sb.AppendLine("(");

        foreach (var column in table.Columns)
        {
            sb.Append($"    [{column.Name}] {column.DataType}");

            if (column.Constraints.Any())
            {
                sb.Append(' ');
                var constraints = string.Join(" ", column.Constraints);
                sb.Append(constraints);
            }

            sb.AppendLine(",");
        }

        sb.AppendLine(")");
        sb.AppendLine("GO");
        sb.AppendLine();
    }

    private static void AddPrimaryKey(StringBuilder sb, Domain.Schema schema, Table table)
    {
        if (table.PrimaryKey == null)
            return;

        sb.AppendLine($"ALTER TABLE [{schema.Name}].[{table.Name}]");
        sb.AppendLine($"    ADD CONSTRAINT [{table.PrimaryKey.Name}]");

        sb.Append("    PRIMARY KEY");

        if (table.PrimaryKey.IsClustered)
            sb.Append(" CLUSTERED");

        sb.AppendLine($" ({string.Join(", ", table.PrimaryKey.Columns)})");

        sb.AppendLine("GO");
        sb.AppendLine();
    }

    private static void AddIndexes(StringBuilder sb, Domain.Schema schema, Table table)
    {
        if (!table.Indexes.Any())
            return;

        foreach (var index in table.Indexes)
        {
            sb.AppendLine($"CREATE INDEX [{index.Name}]");
            sb.Append($"    ON [{schema.Name}].[{table.Name}]");
            sb.AppendLine($" ({string.Join(", ", index.Columns)})");
            sb.AppendLine("GO");
            sb.AppendLine();
        }
    }

    private static void AddColumnstoreIndexes(StringBuilder sb, Schema schema, Table table)
    {
        if (!table.ColumnstoreIndexes.Any())
            return;

        foreach (var index in table.ColumnstoreIndexes)
        {
            if (index.IsClustered)
                sb.AppendLine($"CREATE CLUSTERED COLUMNSTORE INDEX [{index.Name}]");
            else
                sb.AppendLine($"CREATE NONCLUSTERED COLUMNSTORE INDEX [{index.Name}]");
            
            sb.Append($"    ON [{schema.Name}].[{table.Name}]");
            if (index.Order.Any())
            {
                sb.AppendLine(" ORDER (");
                sb.Append(string.Join(',', index.Order));
                sb.Append(")");
            }

            if (index.AdditionalClauses is not null)
            {
                sb.AppendLine($" {index.AdditionalClauses}");
            }

            sb.AppendLine("GO");
            sb.AppendLine();
        }
    }

    private static void AddForeignKeys(Database database, StringBuilder sb)
    {
        sb.AppendLine(ScriptTemplates.FKScriptHeader);

        foreach (var schema in database.Schemas)
        {
            foreach (var table in schema.Tables)
            {
                foreach (var fk in table.ForeignKeys)
                {
                    sb.AppendLine($"ALTER TABLE [{schema.Name}].[{table.Name}]");
                    sb.AppendLine($"    ADD CONSTRAINT [{fk.Name}]");
                    sb.AppendLine($"    FOREIGN KEY ({fk.SourceColumn})");
                    sb.AppendLine($"    REFERENCES [{fk.TargetSchema}].[{fk.TargetTable}] ({fk.TargetColumn})");
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }
            }
        }
    }

    private static void AddMetaData(Database database, StringBuilder sb)
    {
        sb.AppendLine(ScriptTemplates.ExtendedPropertiesHeader);

        foreach (var schema in database.Schemas)
        {
            AddSchemaMetadata(sb, schema);

            foreach (var table in schema.Tables)
            {
                AddTableMetadata(sb, schema, table);
                AddColumnMetadata(sb, schema, table);
            }
        }
    }

    private static void AddSchemaMetadata(StringBuilder sb, Domain.Schema schema)
    {
        if (!string.IsNullOrEmpty(schema.Description))
        {
            sb.AppendLine($"EXECUTE sys.sp_addextendedproperty");
            sb.AppendLine($"  @name = N'{nameof(schema.Description)}',");
            sb.AppendLine($"  @value = N'{Sanitize(schema.Description)}',");
            sb.AppendLine($"  @level0type = N'SCHEMA',");
            sb.AppendLine($"  @level0name = N'{schema.Name}'");
            sb.AppendLine();
        }

        if (!string.IsNullOrEmpty(schema.Notes))
        {
            sb.AppendLine($"EXECUTE sys.sp_addextendedproperty");
            sb.AppendLine($"  @name = N'{nameof(schema.Notes)}',");
            sb.AppendLine($"  @value = N'{Sanitize(schema.Notes)}',");
            sb.AppendLine($"  @level0type = N'SCHEMA',");
            sb.AppendLine($"  @level0name = N'{schema.Name}'");
            sb.AppendLine();
        }

        if (string.IsNullOrEmpty(schema.Description) && string.IsNullOrEmpty(schema.Notes))
            return;

        sb.AppendLine("GO");
        sb.AppendLine();
    }

    private static void AddTableMetadata(StringBuilder sb, Domain.Schema schema, Table table)
    {
        if (!string.IsNullOrEmpty(table.Description))
        {
            sb.AppendLine($"EXECUTE sys.sp_addextendedproperty");
            sb.AppendLine($"  @name = N'{nameof(table.Description)}',");
            sb.AppendLine($"  @value = N'{Sanitize(table.Description)}',");
            sb.AppendLine($"  @level0type = N'SCHEMA',");
            sb.AppendLine($"  @level0name = N'{schema.Name}',");
            sb.AppendLine($"  @level1type = N'TABLE',");
            sb.AppendLine($"  @level1name = N'{table.Name}'");
            sb.AppendLine();
        }

        if (!string.IsNullOrEmpty(table.Notes))
        {
            sb.AppendLine($"EXECUTE sys.sp_addextendedproperty");
            sb.AppendLine($"  @name = N'{nameof(table.Notes)}',");
            sb.AppendLine($"  @value = N'{Sanitize(table.Notes)}',");
            sb.AppendLine($"  @level0type = N'SCHEMA',");
            sb.AppendLine($"  @level0name = N'{schema.Name}',");
            sb.AppendLine($"  @level1type = N'TABLE',");
            sb.AppendLine($"  @level1name = N'{table.Name}'");
            sb.AppendLine();
        }

        if (string.IsNullOrEmpty(table.Description) && string.IsNullOrEmpty(table.Notes))
            return;

        sb.AppendLine("GO");
        sb.AppendLine();
    }

    private static void AddColumnMetadata(StringBuilder sb, Domain.Schema schema, Table table)
    {
        foreach (var column in table.Columns)
        {
            if (!string.IsNullOrEmpty(column.Description))
            {
                sb.AppendLine($"EXECUTE sys.sp_addextendedproperty");
                sb.AppendLine($"  @name = N'{nameof(column.Description)}',");
                sb.AppendLine($"  @value = N'{Sanitize(column.Description)}',");
                sb.AppendLine($"  @level0type = N'SCHEMA',");
                sb.AppendLine($"  @level0name = N'{schema.Name}',");
                sb.AppendLine("  @level1type = N'TABLE',");
                sb.AppendLine($"  @level1name = N'{table.Name}',");
                sb.AppendLine($"  @level2type = N'COLUMN',");
                sb.AppendLine($"  @level2name = N'{column.Name}'");
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(column.Notes))
            {
                sb.AppendLine("EXECUTE sys.sp_addextendedproperty");
                sb.AppendLine($"  @name = N'{nameof(column.Notes)}',");
                sb.AppendLine($"  @value = N'{Sanitize(column.Notes)}',");
                sb.AppendLine($"  @level0type = N'SCHEMA',");
                sb.AppendLine($"  @level0name = N'{schema.Name}',");
                sb.AppendLine($"  @level1type = N'TABLE',");
                sb.AppendLine($"  @level1name = N'{table.Name}',");
                sb.AppendLine($"  @level2type = N'COLUMN',");
                sb.AppendLine($"  @level2name = N'{column.Name}'");
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(column.Source))
            {
                sb.AppendLine($"EXECUTE sys.sp_addextendedproperty");
                sb.AppendLine($"  @name = N'{nameof(column.Source)}',");
                sb.AppendLine($"  @value = N'{Sanitize(column.Source)}',");
                sb.AppendLine($"  @level0type = N'SCHEMA',");
                sb.AppendLine($"  @level0name = N'{schema.Name}',");
                sb.AppendLine($"  @level1type = N'TABLE',");
                sb.AppendLine($"  @level1name = N'{table.Name}',");
                sb.AppendLine($"  @level2type = N'COLUMN',");
                sb.AppendLine($"  @level2name = N'{column.Name}'");
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(column.Example))
            {
                sb.AppendLine($"EXECUTE sys.sp_addextendedproperty");
                sb.AppendLine($"  @name = N'{nameof(column.Example)}',");
                sb.AppendLine($"  @value = N'{Sanitize(column.Example)}',");
                sb.AppendLine($"  @level0type = N'SCHEMA',");
                sb.AppendLine($"  @level0name = N'{schema.Name}',");
                sb.AppendLine($"  @level1type = N'TABLE',");
                sb.AppendLine($"  @level1name = N'{table.Name}',");
                sb.AppendLine($"  @level2type = N'COLUMN',");
                sb.AppendLine($"  @level2name = N'{column.Name}'");
                sb.AppendLine();
            }

            if (string.IsNullOrEmpty(column.Description)
                && string.IsNullOrEmpty(column.Notes)
                && string.IsNullOrEmpty(column.Source)
                && string.IsNullOrEmpty(column.Example))
                continue;

            sb.AppendLine("GO");
            sb.AppendLine();
        }
    }

    private static string Sanitize(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        var lineSeparator = ((char)0x2028).ToString();
        var paragraphSeparator = ((char)0x2029).ToString();

        return value.Replace("\r\n", string.Empty)
            .Replace("\n", string.Empty)
            .Replace("\r", string.Empty)
            .Replace(lineSeparator, string.Empty)
            .Replace(paragraphSeparator, string.Empty)
            .Replace("'", "''");
    }
}