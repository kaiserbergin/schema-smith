using System.Reflection;
using NJsonSchema;
using NJsonSchema.Validation;
using SchemaSmith.IO;

namespace SchemaSmith.Linting;

internal static class JsonSchemaValidator
{
    private static readonly string _jsonSchemaPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!}/Schemas/neo-schema.json";
    
    internal static ICollection<ValidationError> Validate(string filePath)
    {
        var schema = JsonSchema.FromFileAsync(_jsonSchemaPath).GetAwaiter().GetResult();

        return schema.Validate(SpecReader.GetServerSchemaAsJsonFromPath(filePath));
    }
}