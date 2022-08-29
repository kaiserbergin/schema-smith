using SchemaSmith.Domain;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SchemaSmith.IO;

internal static class SpecReader
{
    private static readonly IDeserializer _deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .WithTypeConverter(new YamlStringEnumConverter())
        .Build();

    internal static ServerSchema GetSchemaFromPath(string filePath)
    {
        using var file = new StreamReader(filePath);
        var text = file.ReadToEnd();

        return _deserializer.Deserialize<ServerSchema>(text);
    }
}