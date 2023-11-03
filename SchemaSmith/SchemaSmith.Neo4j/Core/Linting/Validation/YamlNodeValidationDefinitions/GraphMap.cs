using SchemaSmith.Core.Linting.YamlNodes;
using SchemaSmith.Domain.Dto.Validation;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class GraphMap : MappingNodeValidationDefinition
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

    public override List<ValidationEvent> Validate(YamlNode node)
    {
        NeoSchemaTracker.Init();
        return base.Validate(node);
    }
}