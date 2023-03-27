using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal class PropertyMap : MappingNodeValidationDefinition
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