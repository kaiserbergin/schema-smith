using SchemaSmith.Linting.ValidationComponents;
using SchemaSmith.Linting.YamlNodes;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal class RelationshipMap : MappingNodeValidationDefinition
{
    public RelationshipMap()
    {
        RequiredProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "type", new RelationshipType() },
            { "connections", new ConnectionsSequenceScalar() }
        };

        AllowedProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "properties", new PropertySequenceMap() }
        };
    }
    
    internal override List<ValidationEvent> Validate(YamlNode node)
    {
        if (node is not YamlMappingNode mappingNode) 
            return base.Validate(node);

        if (!mappingNode.Children.TryGetValue(new YamlScalarNode("type"), out var typeYamlNode) || typeYamlNode is not YamlScalarNode { Value: { } } typeNode) 
            return base.Validate(node);
        
        var propertyNames = new HashSet<string>();
            
        if (mappingNode.Children.TryGetValue(new YamlScalarNode("properties"), out var properties))
        {
            if (properties is not YamlSequenceNode propertySequence) 
                return base.Validate(node);
                
            foreach (var propertyNode in propertySequence)
            {
                if (propertyNode is not YamlMappingNode propertyMap) 
                    continue;
                if (!propertyMap.Children.TryGetValue(new YamlScalarNode("name"), out var propertyNameNode)) 
                    continue;
                            
                if (propertyNameNode is YamlScalarNode { Value: { } } scalarPropertyNode)
                    propertyNames.Add(scalarPropertyNode.Value);
            }
        }
        
        NeoSchemaTracker.AddRelationship(typeNode.Value, propertyNames);

        return base.Validate(node);
    }
}