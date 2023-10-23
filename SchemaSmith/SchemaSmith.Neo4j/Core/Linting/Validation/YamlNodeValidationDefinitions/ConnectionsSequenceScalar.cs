using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class ConnectionsSequenceScalar : SequenceScalarNodeValidationDefinition
{
    public ConnectionsSequenceScalar()
    {
        ChildValidationDefinition = new ConnectionScalar();
    }
}