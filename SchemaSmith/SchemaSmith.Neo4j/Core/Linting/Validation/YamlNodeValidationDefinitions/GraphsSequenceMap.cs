using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class GraphsSequenceMap : SequenceMapNodeValidationDefinition
{
    public GraphsSequenceMap()
    {
        SequenceItemKey = "name";
        ChildValidationDefinition = new GraphMap();
    }
}