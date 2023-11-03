using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class Index : MappingNodeValidationDefinition
{
    public Index()
    {
        RequiredProperties = new Dictionary<string, NodeValidationDefinition>
        {
            { "name", new IndexName() },
            { "type", new IndexType() },
            { "entity", new IndexEntity() }
        };
    }
}