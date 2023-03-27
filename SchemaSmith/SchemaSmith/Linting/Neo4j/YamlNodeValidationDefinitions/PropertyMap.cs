using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodes;

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