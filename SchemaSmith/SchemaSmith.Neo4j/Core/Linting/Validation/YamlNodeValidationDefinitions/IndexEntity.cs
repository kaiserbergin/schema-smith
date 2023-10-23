using SchemaSmith.Core.Linting.YamlNodes;
using SchemaSmith.Domain.Dto.Validation;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class IndexEntity : MappingNodeValidationDefinition
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

    public override List<ValidationEvent> Validate(YamlNode node)
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