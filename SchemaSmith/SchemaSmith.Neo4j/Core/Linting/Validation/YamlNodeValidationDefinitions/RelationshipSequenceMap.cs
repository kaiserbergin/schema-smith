using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class RelationshipSequenceMap : SequenceMapNodeValidationDefinition
{
    public RelationshipSequenceMap()
    {
        SequenceItemKey = "type";
        ChildValidationDefinition = new RelationshipMap();
    }
}