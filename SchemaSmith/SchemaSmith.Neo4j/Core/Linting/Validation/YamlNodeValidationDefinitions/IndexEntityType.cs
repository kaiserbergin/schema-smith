using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class IndexEntityType : ScalarNodeValidationDefinition
{
    public IndexEntityType()
    {
        AllowedEnumeration = new HashSet<string>
        {
            "node",
            "relationship"
        };
    }
}