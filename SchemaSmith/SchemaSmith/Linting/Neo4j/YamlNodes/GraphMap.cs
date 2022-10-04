using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodes;

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
            { "nodes", new SequenceMapNodeValidationDefinition() },
            { "relationships", new SequenceMapNodeValidationDefinition() },
            { "constraints", new SequenceMapNodeValidationDefinition() },
            { "indexes", new SequenceMapNodeValidationDefinition() },
        };
    }   
}