using System.Text;
using SchemaSmith.Domain.Interfaces;
using SchemaSmith.SQLServer.Domain;

namespace SchemaSmith.SQLServer.Core.DocumentationGeneration;

public class SqlServerDocumentationGenerator : IDocumentationGenerator<Database>
{
    public string Generate(Database database)
    {
        StringBuilder sb = new();

        sb.AppendLine("# Database: " + database.DatabaseName);

        if (!string.IsNullOrEmpty(database.Description))
        {
            sb.AppendLine();
            sb.AppendLine(database.Description);
        }

        foreach (var schema in database.Schemas)
        {
            sb.AppendLine();
            sb.AppendLine("# Schema: " + schema.Name);
            sb.AppendLine();
            sb.AppendLine(schema.Description);
            sb.AppendLine(schema.Notes);

            foreach (var table in schema.Tables)
            {
                AppendTable(sb, table, database);
            }
        }

        return sb.ToString();
    }

    private static void AppendTable(StringBuilder sb, Table table, Database db)
    {
        sb.AppendLine($"## {table.Name}");
        sb.AppendLine();
        sb.AppendLine($"{table.Description}");
        sb.AppendLine($"{table.Notes}");

        AppendRelationshipContext(sb, table, db);

        AppendTableDefinition(sb, table);

        // Append Primary Key info in markdown
        if (table.PrimaryKey != null)
        {
            sb.AppendLine();
            sb.AppendLine("### Primary Key");
            sb.AppendLine();
            sb.AppendLine($"**Name:** {table.PrimaryKey.Name}  ");
            sb.AppendLine($"**Is Clustered?:** {table.PrimaryKey.IsClustered}  ");
            sb.AppendLine($"**Columns:**  ");

            foreach (var colName in table.PrimaryKey.Columns)
            {
                sb.AppendLine("- " + colName);
            }
        }

        if (table.ForeignKeys?.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("### Foreign Keys");
            sb.AppendLine();
            sb.AppendLine("| Name | Source Column | Target Schema | Target Table | Target Column |");
            sb.AppendLine("| --- | --- | --- | --- | --- |");

            foreach (var foreignKey in table.ForeignKeys)
            {
                sb.Append($"| {foreignKey.Name} | ");
                sb.Append($"{foreignKey.SourceColumn} |");
                sb.Append($"{foreignKey.TargetSchema} |");
                sb.Append($"{foreignKey.TargetTable} |");
                sb.Append($"{foreignKey.TargetColumn} |");
                sb.AppendLine();
            }
        }

        if (table.Indexes?.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("### Indexes");
            sb.AppendLine();
            sb.AppendLine("| Name | Is Unique | Columns |");
            sb.AppendLine("| --- | --- | --- |");

            foreach (var index in table.Indexes)
            {
                sb.Append($"| {index.Name} | ");
                sb.Append($"{index.IsUnique} |");
                sb.Append($"{string.Join(", ", index.Columns)} |");
                sb.AppendLine();
            }
        }

        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
    }

    private static void AppendRelationshipContext(StringBuilder sb, Table table, Database db)
    {
        MermaidErdGenerator.Write(db, table, sb);
        sb.AppendLine();
    }

    private static void AppendTableDefinition(StringBuilder sb, Table table)
    {
        sb.AppendLine();
        sb.AppendLine("### Table Definition");
        sb.AppendLine();

        sb.AppendLine(
            "| Column Name | Data Type | PK | FK | Constraints | Description | Example | Source |");
        sb.AppendLine("| --- | --- | --- | --- | --- | --- | --- | --- |");

        foreach (var column in table.Columns)
        {
            var isPk = table.PrimaryKey?.Columns.Contains(column.Name) ?? false;

            var fk = table
                .ForeignKeys?
                .FirstOrDefault(fk => fk.SourceColumn == column.Name);

            string Sanitize(string? value)
            {
                value = value?.Replace("\r", " ");
                value = value?.Replace("\n", " ");
                return value?.Replace("|", "\\|") ?? "";
            }

            sb.Append($"| <sub>{Sanitize(column.Name)}</sub> ");
            sb.Append($"| <sub>{Sanitize(column.DataType)}</sub> ");
            sb.Append($"| <sub>{(isPk ? "X" : "")}</sub> ");
            sb.Append($"| <sub>{(fk != null ? "X" : "")}</sub> ");
            sb.Append($"| <sub>{Sanitize(string.Join(", ", column.Constraints))} ");
            sb.Append($"| <sub>{Sanitize(column.Description)}</sub> ");
            sb.Append($"| <sub>{Sanitize(column.Example)}</sub> ");
            sb.Append($"| <sub>{Sanitize(column.Source)}</sub> |");

            sb.AppendLine();
        }
    }
}