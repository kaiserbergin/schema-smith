using YamlDotNet.RepresentationModel;
using SchemaSmith.Core.Linting.ValidationExtensions;
using SchemaSmith.Domain.Dto.Validation;

namespace SchemaSmith.Core.Linting.YamlNodes;

public class MappingNodeValidationDefinition : NodeValidationDefinition
{
    public override YamlNodeType NodeType => YamlNodeType.Mapping;

    public Dictionary<string, NodeValidationDefinition> RequiredProperties { get; init; } = new();

    public Dictionary<string, NodeValidationDefinition> AllowedProperties { get; init; } = new();

    public ValidationSeverity AllowedPropertiesSeverity = ValidationSeverity.Error;

    public override List<ValidationEvent> Validate(YamlNode node)
    {
        var validationEvents = new List<ValidationEvent>();

        if (!node.Validate(NodeType, validationEvents))
            return validationEvents;

        var mappingNode = (YamlMappingNode)node;

        validationEvents.AddRange(ValidateRequiredProperties(mappingNode));
        validationEvents.AddRange(ValidatePresenceOfAllPropertiesAndChildValidations(mappingNode));

        return validationEvents;
    }

    private List<ValidationEvent> ValidatePresenceOfAllPropertiesAndChildValidations(YamlMappingNode mappingNode)
    {
        var validationEvents = new List<ValidationEvent>();
        
        var requiredAndAllowedPropertiesAndValidations = RequiredProperties
            .Union(AllowedProperties)
            .ToDictionary(kv => kv.Key, kv => kv.Value);
        var requiredAndAllowedProperties = requiredAndAllowedPropertiesAndValidations.Keys.ToHashSet();

        foreach (var yamlNodeKvp in mappingNode.Children)
        {
            if (yamlNodeKvp.Key is YamlScalarNode scalarKeyNode)
            {
                if (scalarKeyNode.Value is null)
                {
                    validationEvents.Add(
                        new ValidationEvent
                        {
                            Severity = ValidationSeverity.Error,
                            Message = $"Expected scalar node for mapping key, but found: \"{scalarKeyNode.NodeType}\" instead.",
                            Position = (scalarKeyNode.Start.Line, scalarKeyNode.Start.Column)
                        }
                    );
                    
                    continue;
                }
                
                if (!requiredAndAllowedProperties.Contains(scalarKeyNode.Value))
                {
                    validationEvents.Add(
                        new ValidationEvent
                        {
                            Severity = AllowedPropertiesSeverity,
                            Message = $"Expected valid property key, but found: \"{scalarKeyNode.Value}\" instead.",
                            Position = (scalarKeyNode.Start.Line, scalarKeyNode.Start.Column)
                        }
                    );
                }
                else
                {
                    var validationDefinition = requiredAndAllowedPropertiesAndValidations
                        .GetValueOrDefault(scalarKeyNode.Value);
                    
                    validationEvents.AddRange(validationDefinition.Validate(yamlNodeKvp.Value));
                }
            }
            else
            {
                validationEvents.Add(
                    new ValidationEvent
                    {
                        Severity = ValidationSeverity.Error,
                        Message = $"Expected scalar node for mapping key, but found: <{yamlNodeKvp.Key.NodeType}> instead.",
                        Position = (yamlNodeKvp.Key.Start.Line, yamlNodeKvp.Key.Start.Column)
                    }
                );
            }
        }

        return validationEvents;
    }

    private List<ValidationEvent> ValidateRequiredProperties(YamlMappingNode mappingNode)
    {
        var validationEvents = new List<ValidationEvent>();
        
        foreach (var requiredProperty in RequiredProperties.Keys)
        {
            if (!mappingNode.Children.TryGetValue(new YamlScalarNode(requiredProperty), out _))
            {
                validationEvents.Add(
                    new ValidationEvent
                    {
                        Severity = ValidationSeverity.Error,
                        Message = $"Did not find required property: \"{requiredProperty}\" in mapping node.",
                        Position = (mappingNode.Start.Line, mappingNode.Start.Column)
                    }
                );
            }
        }

        return validationEvents;
    }
}