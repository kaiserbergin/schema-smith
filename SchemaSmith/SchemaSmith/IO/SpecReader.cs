using SchemaSmith.Domain;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SchemaSmith.IO;

internal static class SpecReader
{
    private static readonly IYamlTypeConverter _converter;
    private static readonly IDeserializer _deserializer;
    private static readonly ISerializer _serializer;
    
    static SpecReader()
    {
        _converter = new YamlStringEnumConverter();
        
        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeConverter(_converter)
            .Build();

        _serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeConverter(_converter)
            .JsonCompatible()
            .Build();
    }

    internal static string GetServerSchemaText(string filePath)
    {
        using var file = new StreamReader(filePath);
        return file.ReadToEnd();
    }

    internal static ServerSchema GetServerSchemaFromPath(string filePath) => 
        _deserializer.Deserialize<ServerSchema>(GetServerSchemaText(filePath));

    internal static string GetServerSchemaAsJsonFromPath(string filePath)
    {
        using var streamReader = new StreamReader(filePath); 
        
        var yamlObject = _deserializer.Deserialize(streamReader) ?? throw new ArgumentNullException("Ya ain't got no file.");

        using var stringWriter = new StringWriter();
        
        _serializer.Serialize(stringWriter, yamlObject);

        return stringWriter.ToString();
    }
}