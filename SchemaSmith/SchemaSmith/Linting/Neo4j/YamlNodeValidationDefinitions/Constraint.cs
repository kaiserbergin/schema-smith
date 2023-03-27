using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal class Constraint : MappingNodeValidationDefinition
{
    public Constraint()
    {
        RequiredProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "name", new ConstraintName() },
            { "type", new ConstraintType() },
            { "entity", new ConstraintEntity() }
        };
    }
}