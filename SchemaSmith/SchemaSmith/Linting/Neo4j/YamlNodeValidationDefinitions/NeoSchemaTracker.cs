using SchemaSmith.Linting.ValidationComponents;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal static class NeoSchemaTracker
{
    private static Dictionary<string, HashSet<string>> _nodes = new();
    private static Dictionary<string, HashSet<string>> _relationships = new();

    internal static void Init()
    {
        _nodes = new Dictionary<string, HashSet<string>>();
        _relationships = new Dictionary<string, HashSet<string>>();
    }

    internal static IReadOnlyDictionary<string, HashSet<string>> Nodes => _nodes;

    internal static void AddNode(string label, HashSet<string> propertyNames) => _nodes.Add(label, propertyNames);
    
    internal static IReadOnlyDictionary<string, HashSet<string>> Relationships => _relationships;

    internal static void AddRelationship(string type, HashSet<string> propertyNames) => _relationships.Add(type, propertyNames);
    
    internal static List<ValidationEvent> ValidateEntityReferences(string entityType, YamlScalarNode idScalarNode, YamlMappingNode mappingNode)
    {
        var validationEvents = new List<ValidationEvent>();
        var entityProperties = new HashSet<string>();

        if (entityType == "node" && Nodes.TryGetValue(idScalarNode.Value, out entityProperties)
            || entityType == "relationship" && Relationships.TryGetValue(idScalarNode.Value, out entityProperties))
        {
            if (mappingNode.Children.TryGetValue(new YamlScalarNode("properties"), out var propertiesNode)
                && propertiesNode is YamlSequenceNode propertiesSequenceNode
                && propertiesSequenceNode.Children.Count > 0)
            {
                foreach (var propertyScalar in propertiesSequenceNode.Children.OfType<YamlScalarNode>())
                {
                    if (propertyScalar.Value is not null && !entityProperties.Contains(propertyScalar.Value))
                    {
                        validationEvents.Add(
                            new ValidationEvent
                            {
                                Severity = ValidationSeverity.Error,
                                Message = $"Index defined for property of \"{propertyScalar.Value}\" on {entityType} \"{idScalarNode.Value}\", but no such property is defined.",
                                Position = (propertyScalar.Start.Line, propertyScalar.Start.Column)
                            }
                        );
                    }
                }
            }
        }
        else
        {
            validationEvents.Add(
                new ValidationEvent
                {
                    Severity = ValidationSeverity.Error,
                    Message = $"Index defined for {entityType} \"{idScalarNode.Value}\", but no {entityType} is defined.",
                    Position = (idScalarNode.Start.Line, idScalarNode.Start.Column)
                }
            );
        }

        return validationEvents;
    }
}