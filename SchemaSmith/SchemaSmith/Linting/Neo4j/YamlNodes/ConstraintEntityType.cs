using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodes;

internal class ConstraintEntityType : ScalarNodeValidationDefinition
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