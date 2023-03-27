using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal class MapNodesSequenceMap : SequenceMapNodeValidationDefinition
{
    public MapNodesSequenceMap()
    {
        SequenceItemKey = "label";
        ChildValidationDefinition = new NodeMap();
    }
}