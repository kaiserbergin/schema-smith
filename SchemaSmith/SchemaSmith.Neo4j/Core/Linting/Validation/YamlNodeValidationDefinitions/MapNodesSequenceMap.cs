using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class MapNodesSequenceMap : SequenceMapNodeValidationDefinition
{
    public MapNodesSequenceMap()
    {
        SequenceItemKey = "label";
        ChildValidationDefinition = new NodeMap();
    }
}