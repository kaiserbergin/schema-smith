using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal class ConstraintType : ScalarNodeValidationDefinition
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