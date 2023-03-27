using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal class IndexType : ScalarNodeValidationDefinition
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