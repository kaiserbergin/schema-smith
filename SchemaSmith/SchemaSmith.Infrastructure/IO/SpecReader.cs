using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SchemaSmith.Infrastructure.IO;

public static class SpecReader
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

    public static YamlMappingNode GetYamlMapping(string filePath)
    {
        // Setup the input
        var input = new StringReader(GetServerSchemaText(filePath));

        // Load the stream
        var yaml = new YamlStream();
        yaml.Load(input);

        // Examine the stream
        var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
       
        return mapping;
    }

    public static string GetServerSchemaText(string filePath)
    {
        using var file = new StreamReader(filePath);
        return file.ReadToEnd();
    }

    public static T GetServerSchemaFromPath<T>(string filePath) => 
        _deserializer.Deserialize<T>(GetServerSchemaText(filePath));

    public static string GetServerSchemaAsJsonFromPath(string filePath)
    {
        using var streamReader = new StreamReader(filePath); 
        
        var yamlObject = _deserializer.Deserialize(streamReader) ?? throw new ArgumentNullException("Ya ain't got no file.");

        using var stringWriter = new StringWriter();
        
        _serializer.Serialize(stringWriter, yamlObject);

        return stringWriter.ToString();
    }
}