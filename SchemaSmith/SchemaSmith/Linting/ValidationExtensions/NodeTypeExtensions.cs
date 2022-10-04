using SchemaSmith.Linting.ValidationComponents;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Linting.ValidationExtensions;

internal static class NodeTypeExtensions
{
    internal static bool Validate(this YamlNode node, YamlNodeType expectedType, List<ValidationEvent> validationEvents)
    {
        if (node.Validate(expectedType, out var validationEvent)) 
            return true;
        
        validationEvents.Add(validationEvent!);
        return false;

    }

    internal static bool Validate(this YamlNode node, YamlNodeType expectedType, out ValidationEvent? validationEvent)
    {
        validationEvent = null;

        if (node.NodeType == expectedType)
            return true;

        validationEvent = new ValidationEvent
        {
            Severity = ValidationSeverity.Error,
            Message = $"Expected YamlNodeType of {expectedType}, but found {node.NodeType}.",
            Position = (node.Start.Line, node.Start.Column)
        };

        return false;
    }
}