// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Serialization;
using Namotion.Reflection;
using Newtonsoft.Json;
using NJsonSchema;
using NJsonSchema.Generation;
using SchemaSmith.Domain;

var settings = new JsonSchemaGeneratorSettings
{
    SerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    },
};

var schema = JsonSchema.FromType<ServerSchema>(settings);
var schemaData = schema.ToJson(Formatting.Indented);

using var outputFile = File.Open($"schema-smith.schema.json", FileMode.Create);
using var streamWriter = new StreamWriter(outputFile);

streamWriter.Write(schemaData);

streamWriter.Close();
outputFile.Close();

var test = true;