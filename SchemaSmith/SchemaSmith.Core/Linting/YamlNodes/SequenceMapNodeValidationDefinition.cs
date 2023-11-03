using SchemaSmith.Core.Linting.ValidationExtensions;
using SchemaSmith.Domain.Dto.Validation;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Core.Linting.YamlNodes;

public class SequenceMapNodeValidationDefinition : NodeValidationDefinition
{
    public override YamlNodeType NodeType => YamlNodeType.Sequence;

    public MappingNodeValidationDefinition ChildValidationDefinition { get; init; } = new();
    
    public string SequenceItemKey { get; init; } = null!;

    public ValidationSeverity SequenceItemKeySeverity = ValidationSeverity.Error;

    public override List<ValidationEvent> Validate(YamlNode node)
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
                validationEvents.AddRange(ChildValidationDefinition.Validate(childNode));
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
                    Severity = SequenceItemKeySeverity,
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
                    Severity = SequenceItemKeySeverity,
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