using System.Reflection;
using Newtonsoft.Json;
using NJsonSchema;
using NJsonSchema.Validation;
using SchemaSmith.Domain;
using SchemaSmith.IO;

namespace SchemaSmith.Linting;

internal static class ServerSchemaValidator
{
    private static readonly string _jsonSchemaPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!}/Schemas/neo-schema.json";
    
    internal static IEnumerable<ValidationError> Validate(string filePath)
    {
        var schema = JsonSchema.FromFileAsync(_jsonSchemaPath).GetAwaiter().GetResult();
        var json = SpecReader.GetServerSchemaAsJsonFromPath(filePath);

        return schema.Validate(json);
    }
}