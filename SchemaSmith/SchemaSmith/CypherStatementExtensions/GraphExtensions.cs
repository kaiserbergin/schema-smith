﻿using SchemaSmith.Domain;

namespace SchemaSmith.CypherStatementExtensions;

public static class GraphExtensions
{
    internal static IEnumerable<string> GenerateCypherStatements(this Graph graph)
    {
        var statements = new List<string>();
        
        statements.AddRange(graph.Constraints.Select(x => x.GenerateCypher()));
        statements.AddRange(graph.Indexes.Select(x => x.GenerateCypher()));
        statements.AddRange(graph.Nodes.Select(x => x.GenerateCypher()));
        statements.AddRange(graph.Relationships.SelectMany(x => x.GenerateCypher()));

        return statements;
    }
}