using SchemaSmith.Linting.ValidationComponents;
using SchemaSmith.Linting.YamlNodes;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Linting.Neo4j.YamlNodeValidationDefinitions;

internal class GraphMap : MappingNodeValidationDefinition
{
    public GraphMap()
    {
        RequiredProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "name", new ScalarNodeValidationDefinition() },
        };
        
        AllowedProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "nodes", new MapNodesSequenceMap() },
            { "relationships", new RelationshipSequenceMap() },
            { "constraints", new ConstraintSequence() },
            { "indexes", new IndexSequence() },
        };
    }

    internal override List<ValidationEvent> Validate(YamlNode node)
    {
        NeoSchemaTracker.Init();
        return base.Validate(node);
    }
}