using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodes;

internal class MapNodesSequenceMap : SequenceMapNodeValidationDefinition
{
    public MapNodesSequenceMap()
    {
        SequenceItemKey = "label";
        ChildValidationDefinition = new NodeMap();
    }
}