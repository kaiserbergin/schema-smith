using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodes;

internal class ConnectionsSequenceScalar : SequenceScalarNodeValidationDefinition
{
    public ConnectionsSequenceScalar()
    {
        ChildValidationDefinition = new ConnectionScalar();
    }
}