using SchemaSmith.IO;
using SchemaSmith.Linting.ValidationComponents;

namespace SchemaSmith.Linting.Neo4j.Validation;

internal class NeoSpecValidator
{
    internal static List<ValidationEvent> ValidateNeo4jSpec(string filename)
    {
        var validationEvents = new List<ValidationEvent>();
        
        var root = SpecReader.GetYamlStream(filename);
        
        

        return validationEvents;
    }
}