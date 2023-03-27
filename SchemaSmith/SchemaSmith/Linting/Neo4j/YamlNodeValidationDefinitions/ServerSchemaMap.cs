using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodes;

internal class ServerSchemaMap : MappingNodeValidationDefinition
{
    public ServerSchemaMap()
    {
        RequiredProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "graphs", new GraphsSequenceMap() }
        };
        
        AllowedProperties = new Dictionary<string, NodeValidationDefinition>
        {
            {"serverUrl", new ScalarNodeValidationDefinition() }
        };
    }
}