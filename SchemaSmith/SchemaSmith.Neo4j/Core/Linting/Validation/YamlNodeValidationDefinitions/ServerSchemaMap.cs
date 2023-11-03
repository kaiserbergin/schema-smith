using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class ServerSchemaMap : MappingNodeValidationDefinition
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