using SchemaSmith.Neo4j.Domain.Dto;
using SchemaSmith.Neo4j.Infrastructure.Queries.Provider;

namespace SchemaSmith.Neo4j.Core.ScriptGeneration.ExtensionMethods;

public static class GraphExtensions
{
    public static IEnumerable<string> GenerateCypherStatements(this GraphSchema graphSchema)
    {
        var statements = new List<string>();
        
        statements.Add($":use {graphSchema.Name};\n");
        statements.AddRange(graphSchema.Constraints.Select(x => x.GenerateCypher()));
        statements.AddRange(graphSchema.Indexes.Select(x => x.GenerateCypher()));
        statements.AddRange(graphSchema.Nodes.Select(x => x.GenerateCypher()));
        statements.AddRange(graphSchema.Relationships.SelectMany(x => x.GenerateCypher()));
        // Yes, leaked a bit here... it's FIIIINE.
        statements.Add(QueryProvider.DeleteSchemaSmithEntities);

        return statements;
    }
}