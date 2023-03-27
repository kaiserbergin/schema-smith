using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodes;

internal class IndexSequence : SequenceMapNodeValidationDefinition
{
    public IndexSequence()
    {
        SequenceItemKey = "name";
        ChildValidationDefinition = new Index();
    }
}