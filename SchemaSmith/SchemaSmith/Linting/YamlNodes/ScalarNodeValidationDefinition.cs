using System.Text.RegularExpressions;
using SchemaSmith.Linting.Styles;
using SchemaSmith.Linting.ValidationComponents;
using SchemaSmith.Linting.ValidationExtensions;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Linting.YamlNodes;

internal class ScalarNodeValidationDefinition : NodeValidationDefinition
{
    internal override YamlNodeType NodeType => YamlNodeType.Scalar;

    internal virtual CaseType CaseType { get; init; } = CaseType.Any;

    internal HashSet<string>? AllowedEnumeration { get; init; }
    
    internal string? RegexConstraint { get; set; }

    internal override List<ValidationEvent> Validate(YamlNode node)
    {
        var validationEvents = new List<ValidationEvent>();
        
        if (!node.Validate(NodeType, validationEvents))
            return validationEvents;

        if (CaseType is CaseType.Any)
            return validationEvents;

        var scalarNode = (YamlScalarNode)node;
        var casing = CaseChecker.GetCase(scalarNode.Value);

        if (!CaseType.HasFlag(casing))
        {
            validationEvents.Add(
                new ValidationEvent
                {
                    Severity = ValidationSeverity.Error,
                    Message = $"Expected casing style of [{CaseType}] but found [{casing}] for \"{scalarNode.Value}\".",
                    Position = (scalarNode.Start.Line, node.Start.Column)
                }
            );
        }

        if (AllowedEnumeration is not null && scalarNode.Value is not null && !AllowedEnumeration.Contains(scalarNode.Value))
        {
            validationEvents.Add(
                new ValidationEvent
                {
                    Severity = ValidationSeverity.Error,
                    Message = $"Expected valid value for entry, but found \"{scalarNode.Value}\". Valid values are: \n[{string.Join(',', AllowedEnumeration)}].",
                    Position = (scalarNode.Start.Line, node.Start.Column)
                }
            );
        }

        if (RegexConstraint is not null && scalarNode.Value is not null && !Regex.Match(scalarNode.Value, RegexConstraint).Success)
        {
            validationEvents.Add(
                new ValidationEvent
                {
                    Severity = ValidationSeverity.Error,
                    Message = $"Expected valid value for entry, but found \"{scalarNode.Value}\". Valid values should match: {RegexConstraint}",
                    Position = (scalarNode.Start.Line, node.Start.Column)
                }
            );
        }
        

        return validationEvents;
    }
}