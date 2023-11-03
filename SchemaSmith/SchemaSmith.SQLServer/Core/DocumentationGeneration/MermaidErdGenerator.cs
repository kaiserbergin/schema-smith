using System.Text;
using SchemaSmith.SQLServer.Domain;

namespace SchemaSmith.SQLServer.Core.DocumentationGeneration;

public class MermaidErdGenerator
{
    public static void Write(SqlDatabase sqlDatabase, Table focusTable, StringBuilder ogSb)
    {
        var fkSb = new StringBuilder();
        
        var parentSchema = sqlDatabase.Schemas
            .Single(schema => schema.Tables.Contains(focusTable));

        AppendFks(focusTable, fkSb, parentSchema);

        AppendReferences(fkSb, sqlDatabase, focusTable, parentSchema);

        if (fkSb.Length > 0)
        {
            ogSb.AppendLine();
            ogSb.AppendLine("### Relationship Context");
            ogSb.AppendLine();
            ogSb.AppendLine("```mermaid");
            ogSb.AppendLine("erDiagram"); 
            ogSb.Append(fkSb);
            ogSb.AppendLine("```");
            ogSb.AppendLine();
        }
    }

    private static void AppendReferences(
        StringBuilder sb, 
        SqlDatabase sqlDatabase, 
        Table focusTable, 
        Domain.Schema parentSchema)
    {
        var schemaTableFk = new Dictionary<string, Dictionary<string, (Table, ForeignKey)>>();

        foreach (var schema in sqlDatabase.Schemas)
        {
            foreach (var table in schema.Tables)
            {
                foreach (var fk in table.ForeignKeys)
                {
                    if (fk.TargetSchema == parentSchema.Name && fk.TargetTable == focusTable.Name)
                    {
                        if (!schemaTableFk.ContainsKey(schema.Name))
                            schemaTableFk[schema.Name] = new();

                        if (!schemaTableFk[schema.Name].ContainsKey(table.Name))
                        {
                            schemaTableFk[schema.Name][table.Name] = (table, fk);
                        }
                    }
                }
            }
        }

        foreach (var schema in schemaTableFk.Keys)
        {
            foreach (var table in schemaTableFk[schema].Keys)
            {
                var (refTable, fk) = schemaTableFk[schema][table];
                
                if (refTable.PrimaryKey?.Columns.Count == 1 &&
                    refTable.PrimaryKey?.Columns.Any(pk => pk == fk.SourceColumn) == true)
                {
                    sb.AppendLine(
                        $"""  "{parentSchema.Name}.{focusTable.Name}" ||--|| "{schema}.{refTable.Name}" : "" """);
                }
                else
                {
                    sb.AppendLine(
                        $$"""  "{{parentSchema.Name}}.{{focusTable.Name}}" ||--o{ "{{schema}}.{{refTable.Name}}" : "" """);
                }
            }
        }
    }

    private static void AppendFks(Table focusTable, StringBuilder sb, Domain.Schema parentSchema)
    {
        var tableFks = focusTable.ForeignKeys
            .ToLookup(fk => fk.TargetTable);

        // Mermaid ERD looks ugly with multiple ties to the same table.
        //  This is just a simplistic view, so we aren't concerned with perfection.
        foreach (var fkGroup in tableFks)
        {
            var fk = fkGroup.First();
            if (focusTable.PrimaryKey?.Columns.Count == 1 &&
                focusTable.PrimaryKey?.Columns.Any(pk => pk == fk.SourceColumn) == true)
            {
                sb.AppendLine(
                    $"""  "{parentSchema.Name}.{focusTable.Name}" ||--|| "{fk.TargetSchema}.{fk.TargetTable}" : "" """);
            }
            else
            {
                sb.AppendLine(
                    $$"""  "{{parentSchema.Name}}.{{focusTable.Name}}" }o--|| "{{fk.TargetSchema}}.{{fk.TargetTable}}" : "" """);
            }
        }
    }
}