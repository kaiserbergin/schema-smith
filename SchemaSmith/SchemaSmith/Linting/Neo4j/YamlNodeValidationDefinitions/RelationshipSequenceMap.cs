using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal class RelationshipSequenceMap : SequenceMapNodeValidationDefinition
{
    public RelationshipSequenceMap()
    {
        SequenceItemKey = "type";
        ChildValidationDefinition = new RelationshipMap();
    }
}