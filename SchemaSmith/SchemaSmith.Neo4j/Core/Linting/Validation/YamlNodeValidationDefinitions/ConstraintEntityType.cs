using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class ConstraintEntityType : ScalarNodeValidationDefinition
{
    public ConstraintEntityType()
    {
        AllowedEnumeration = new HashSet<string>
        {
            "node",
            "relationship"
        };
    }
}