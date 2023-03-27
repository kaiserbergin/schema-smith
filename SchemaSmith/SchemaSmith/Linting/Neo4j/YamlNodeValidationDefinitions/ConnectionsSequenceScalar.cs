using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal class ConnectionsSequenceScalar : SequenceScalarNodeValidationDefinition
{
    public ConnectionsSequenceScalar()
    {
        ChildValidationDefinition = new ConnectionScalar();
    }
}