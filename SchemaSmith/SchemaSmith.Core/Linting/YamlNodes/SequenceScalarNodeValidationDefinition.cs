using SchemaSmith.Core.Linting.ValidationExtensions;
using SchemaSmith.Domain.Dto.Validation;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Core.Linting.YamlNodes;

public class SequenceScalarNodeValidationDefinition : NodeValidationDefinition
{
    public override YamlNodeType NodeType => YamlNodeType.Sequence;

    public ScalarNodeValidationDefinition ChildValidationDefinition { get; init; } = new();

    public override List<ValidationEvent> Validate(YamlNode node)
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