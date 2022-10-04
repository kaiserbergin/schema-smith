using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodes;

internal class RelationshipMap : MappingNodeValidationDefinition
{
    public RelationshipMap()
    {
        RequiredProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "type", new RelationshipType() },
            { "connections", new ScalarNodeValidationDefinition() }
        };

        AllowedProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "properties", new SequenceMapNodeValidationDefinition() }
        };
    }
}