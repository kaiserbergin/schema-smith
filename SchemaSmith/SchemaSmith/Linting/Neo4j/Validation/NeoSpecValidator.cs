using SchemaSmith.IO;
using SchemaSmith.Linting.Neo4j.YamlNodes;
using SchemaSmith.Linting.ValidationComponents;

namespace SchemaSmith.Linting.Neo4j.Validation;

internal static class NeoSpecValidator
{
    internal static List<ValidationEvent> ValidateNeo4jSpec(string filename)
    {
        var root = SpecReader.GetYamlMapping(filename);

        var validator = new ServerSchemaMap();

        return validator.Validate(root);
    }
}