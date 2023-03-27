using System.Text;
using SchemaSmith.Domain;
using YamlDotNet.RepresentationModel;
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

    internal static YamlMappingNode GetYamlMapping(string filePath)
    {
        var sb = new StringBuilder();
        
        // Setup the input
        var input = new StringReader(GetServerSchemaText(filePath));

        // Load the stream
        var yaml = new YamlStream();
        yaml.Load(input);

        // Examine the stream
        var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
        
        foreach (var entry in mapping.Children)
        {
            sb.AppendLine();
            
            sb.AppendLine(entry.Value.NodeType.ToString());
            
            sb.AppendLine($"Start Line: {entry.Key.Start.Line}");
            sb.AppendLine($"Start Column: {entry.Key.Start.Column}");
            sb.AppendLine($"Start Index: {entry.Key.Start.Index}");
            
            sb.AppendLine($"End Line: {entry.Key.End.Line}");
            sb.AppendLine($"End Column: {entry.Key.End.Column}");
            sb.AppendLine($"End Index: {entry.Key.End.Index}");
            
            sb.AppendLine(((YamlScalarNode)entry.Key).Value);
            sb.AppendLine(entry.Key.ToString());
        }
        
        var test = sb.ToString();

        return mapping;
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