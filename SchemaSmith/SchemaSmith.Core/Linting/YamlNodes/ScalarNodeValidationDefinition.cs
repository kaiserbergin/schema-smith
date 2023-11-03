using System.Text.RegularExpressions;
using SchemaSmith.Core.Linting.Styles;
using SchemaSmith.Core.Linting.ValidationExtensions;
using SchemaSmith.Domain.Dto.Validation;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Core.Linting.YamlNodes;

public class ScalarNodeValidationDefinition : NodeValidationDefinition
{
    public override YamlNodeType NodeType => YamlNodeType.Scalar;

    public virtual CaseType CaseType { get; init; } = CaseType.Any;

    public ValidationSeverity CaseTypeSeverity { get; init; } = ValidationSeverity.Warning;

    public HashSet<string>? AllowedEnumeration { get; init; }

    public ValidationSeverity AllowedEnumerationSeverity = ValidationSeverity.Error;
    
    public string? RegexConstraint { get; set; }

    public ValidationSeverity RegexConstraintSeverity = ValidationSeverity.Error;

    public override List<ValidationEvent> Validate(YamlNode node)
    {
        var validationEvents = new List<ValidationEvent>();
        
        if (!node.Validate(NodeType, validationEvents))
            return validationEvents;
        
        var scalarNode = (YamlScalarNode)node;
        var casing = CaseChecker.GetCase(scalarNode.Value);

        if (CaseType is not CaseType.Any && (CaseType & casing) == 0)
        {
            validationEvents.Add(
                new ValidationEvent
                {
                    Severity = CaseTypeSeverity,
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
                    Severity = AllowedEnumerationSeverity,
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
                    Severity = RegexConstraintSeverity,
                    Message = $"Expected valid value for entry, but found \"{scalarNode.Value}\". Valid values should match: {RegexConstraint}",
                    Position = (scalarNode.Start.Line, node.Start.Column)
                }
            );
        }
        

        return validationEvents;
    }
}