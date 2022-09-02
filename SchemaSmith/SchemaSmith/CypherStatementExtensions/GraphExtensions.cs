using SchemaSmith.Domain;
using SchemaSmith.Queries.Provider;

namespace SchemaSmith.CypherStatementExtensions;

public static class GraphExtensions
{
    internal static IEnumerable<string> GenerateCypherStatements(this GraphSchema graphSchema)
    {
        var statements = new List<string>();
        
        statements.Add($":use {graphSchema.Name};\n");
        statements.AddRange(graphSchema.Constraints.Select(x => x.GenerateCypher()));
        statements.AddRange(graphSchema.Indexes.Select(x => x.GenerateCypher()));
        statements.AddRange(graphSchema.Nodes.Select(x => x.GenerateCypher()));
        statements.AddRange(graphSchema.Relationships.SelectMany(x => x.GenerateCypher()));
        statements.Add(QueryProvider.DeleteSchemaSmithEntities);

        return statements;
    }
}