using SchemaSmith.Core.Linting.YamlNodes;
using SchemaSmith.Domain.Dto.Validation;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class NodeMap : MappingNodeValidationDefinition
{
    public NodeMap()
    {
        RequiredProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "label", new LabelName() }
        };
        
        AllowedProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "properties", new PropertySequenceMap() }
        };
    }

    public override List<ValidationEvent> Validate(YamlNode node)
    {
        if (node is not YamlMappingNode mappingNode) 
            return base.Validate(node);

        if (!mappingNode.Children.TryGetValue(new YamlScalarNode("label"), out var labelNode) || labelNode is not YamlScalarNode { Value: { } } label) 
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
        
        NeoSchemaTracker.AddNode(label.Value, propertyNames);

        return base.Validate(node);
    }
}