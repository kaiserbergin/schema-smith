using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class ConnectionScalar : ScalarNodeValidationDefinition
{
    public ConnectionScalar()
    {
        RegexConstraint = "(?<=^[A-Z][A-Z|a-z|/d]*)(->|<-)(?=[A-Z][A-Z|a-z|/d]*$)";
    }
}