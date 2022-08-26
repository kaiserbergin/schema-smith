using SchemaSmith.Domain;

namespace SchemaSmith.CypherGenerator;

public static class CypherGenerator
{
    public static IEnumerable<string> CreateConstraints(IEnumerable<Constraint> constraints)
        => constraints.Select(constraint => constraint.GenerateCypher());

}