using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal class IndexSequence : SequenceMapNodeValidationDefinition
{
    public IndexSequence()
    {
        SequenceItemKey = "name";
        ChildValidationDefinition = new Index();
    }
}