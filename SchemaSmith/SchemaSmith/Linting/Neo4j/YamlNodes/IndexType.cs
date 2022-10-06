using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodes;

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