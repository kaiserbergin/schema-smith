using Namotion.Reflection;
using SchemaSmith.Linting.ValidationComponents;
using SchemaSmith.Linting.YamlNodes;
using YamlDotNet.Core.Tokens;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Linting.Neo4j.YamlNodes;

internal class IndexEntity : MappingNodeValidationDefinition
{
    public IndexEntity()
    {
        RequiredProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "type", new IndexEntityType() },
            { "id", new ScalarNodeValidationDefinition() },
            { "properties", new SequenceScalarNodeValidationDefinition() }
        };
    }

    internal override List<ValidationEvent> Validate(YamlNode node)
    {
        var validationEvents = new List<ValidationEvent>();

        if (node is YamlMappingNode mappingNode
            && mappingNode.Children.TryGetValue(new YamlScalarNode("type"), out var typeNode)
            && typeNode is YamlScalarNode { Value: "node" or "relationship" } typeScalarNode)
        {
            if (mappingNode.Children.TryGetValue(new YamlScalarNode("id"), out var idNode)
                && idNode is YamlScalarNode { Value: { } } idScalarNode)
            {
                validationEvents.AddRange(NeoSchemaTracker.ValidateEntityReferences(typeScalarNode.Value, idScalarNode, mappingNode));
            }
        }

        validationEvents.AddRange(base.Validate(node));

        return validationEvents;
    }
}