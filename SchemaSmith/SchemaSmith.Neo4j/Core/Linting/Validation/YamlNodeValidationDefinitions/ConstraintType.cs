using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class ConstraintType : ScalarNodeValidationDefinition
{
    public ConstraintType()
    {
        AllowedEnumeration = new HashSet<string>
        {
            "node-key",
            "existence",
            "unique"
        };
    }
}