using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodes;

internal class NodeMap : MappingNodeValidationDefinition
{
    public NodeMap()
    {
        RequiredProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "label", new LabelName() }
        };
        
        AllowedProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "properties", new SequenceMapNodeValidationDefinition() }
        };
    }
}