using SchemaSmith.Linting.ValidationComponents;
using SchemaSmith.Linting.ValidationExtensions;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Linting.YamlNodes;

internal class SequenceScalarNodeValidationDefinition : NodeValidationDefinition
{
    internal override YamlNodeType NodeType => YamlNodeType.Sequence;

    internal ScalarNodeValidationDefinition ChildValidationDefinition { get; init; } = new();

    internal override List<ValidationEvent> Validate(YamlNode node)
    {
        var validationEvents = new List<ValidationEvent>();

        if (!node.Validate(NodeType, validationEvents))
            return validationEvents;
        
        var sequenceNode = (YamlSequenceNode)node;
        
        foreach (var childNode in sequenceNode.Children)
        {
            if (childNode is YamlScalarNode scalarNode)
            {
                validationEvents.AddRange(ChildValidationDefinition.Validate(childNode));
            }
            else
            {
                validationEvents.Add(
                    new ValidationEvent
                    {
                        Severity = ValidationSeverity.Error,
                        Message = $"Expected sequence of scalar nodes, but found node of <{childNode.NodeType}> type.",
                        Position = (childNode.Start.Line, childNode.Start.Column)
                    }
                );
            }
        }

        return validationEvents;
    }
}