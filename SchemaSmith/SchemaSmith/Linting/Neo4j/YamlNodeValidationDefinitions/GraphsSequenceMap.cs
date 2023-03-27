using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal class GraphsSequenceMap : SequenceMapNodeValidationDefinition
{
    public GraphsSequenceMap()
    {
        SequenceItemKey = "name";
        ChildValidationDefinition = new GraphMap();
    }
}