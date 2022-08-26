using SchemaSmith.Domain;

namespace SchemaSmith.CypherGenerator;

internal static class CypherGenerator
{
    internal static IEnumerable<string> CreateConstraints(IEnumerable<Constraint> constraints)
        => constraints.Select(constraint => constraint.GenerateCypher());

}