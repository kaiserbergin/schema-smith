using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal class IndexEntityType : ScalarNodeValidationDefinition
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