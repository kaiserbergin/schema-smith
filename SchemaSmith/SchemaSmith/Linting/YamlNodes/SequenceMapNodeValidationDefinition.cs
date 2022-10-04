using SchemaSmith.Linting.ValidationComponents;
using SchemaSmith.Linting.ValidationExtensions;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Linting.YamlNodes;

internal class SequenceMapNodeValidationDefinition : NodeValidationDefinition
{
    internal override YamlNodeType NodeType => YamlNodeType.Sequence;

    internal MappingNodeValidationDefinition ChildValidationDefinition { get; init; } = new();
    
    internal string SequenceItemKey { get; init; } = null!;

    internal override List<ValidationEvent> Validate(YamlNode node)
    {
        var validationEvents = new List<ValidationEvent>();

        if (!node.Validate(NodeType, validationEvents))
            return validationEvents;
        
        var sequenceNode = (YamlSequenceNode)node;
        var usedKeys = new HashSet<string>();
        
        foreach (var childNode in sequenceNode.Children)
        {
            if (childNode is YamlMappingNode yamlMappingNode)
            {
                ValidateMappingNode(yamlMappingNode, validationEvents, usedKeys);
            }
            else
            {
                validationEvents.Add(
                    new ValidationEvent
                    {
                        Severity = ValidationSeverity.Error,
                        Message = $"Expected sequence of mapping nodes, but found node of <{childNode.NodeType}> type.",
                        Position = (childNode.Start.Line, childNode.Start.Column)
                    }
                );
            }
        }
        
        return validationEvents;
    }

    private void ValidateMappingNode(YamlMappingNode yamlMappingNode, List<ValidationEvent> validationEvents, HashSet<string> usedKeys)
    {
        if (!yamlMappingNode.Children.TryGetValue(new YamlScalarNode(SequenceItemKey), out var keyProperty))
        {
            validationEvents.Add(
                new ValidationEvent
                {
                    Severity = ValidationSeverity.Error,
                    Message = $"Expected sequence item key of {SequenceItemKey}, but found none.",
                    Position = (yamlMappingNode.Start.Line, yamlMappingNode.Start.Column)
                }
            );

            return;
        }

        var keyValue = keyProperty.ToString();

        if (usedKeys.Contains(keyValue))
        {
            validationEvents.Add(
                new ValidationEvent
                {
                    Severity = ValidationSeverity.Error,
                    Message = $"Duplicate sequence key value of \"{keyValue}\" for property {SequenceItemKey}.",
                    Position = (keyProperty.Start.Line, keyProperty.Start.Column)
                }
            );
        }
        else
        {
            usedKeys.Add(keyValue);
        }
    }
}