using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal class ConnectionScalar : ScalarNodeValidationDefinition
{
    public ConnectionScalar()
    {
        RegexConstraint = "(?<=^[A-Z][A-Z|a-z|/d]*)(->|<-)(?=[A-Z][A-Z|a-z|/d]*$)";
    }
}