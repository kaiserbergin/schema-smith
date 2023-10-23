using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class Constraint : MappingNodeValidationDefinition
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