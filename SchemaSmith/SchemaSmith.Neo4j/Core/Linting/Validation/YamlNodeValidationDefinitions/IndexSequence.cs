using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class IndexSequence : SequenceMapNodeValidationDefinition
{
    public IndexSequence()
    {
        SequenceItemKey = "name";
        ChildValidationDefinition = new Index();
    }
}