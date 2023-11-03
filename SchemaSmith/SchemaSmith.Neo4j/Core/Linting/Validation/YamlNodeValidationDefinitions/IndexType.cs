using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class IndexType : ScalarNodeValidationDefinition
{
    public IndexType()
    {
        AllowedEnumeration = new HashSet<string>
        {
            "b-tree",
            "text",
            "range",
            "point",
        };
    }
}