using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class PropertyMap : MappingNodeValidationDefinition
{
    public PropertyMap()
    {
        RequiredProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "name", new PropertyKey() },
            { "type", new PropertyScalarType()}
        };
    }
}