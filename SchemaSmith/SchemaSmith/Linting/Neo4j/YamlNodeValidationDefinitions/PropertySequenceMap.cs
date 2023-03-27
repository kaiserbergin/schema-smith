using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal class PropertySequenceMap : SequenceMapNodeValidationDefinition
{
    public PropertySequenceMap()
    {
        SequenceItemKey = "name";
        ChildValidationDefinition = new PropertyMap();
    }
}