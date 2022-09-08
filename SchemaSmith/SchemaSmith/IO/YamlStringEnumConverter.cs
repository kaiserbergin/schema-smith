using System.Reflection;
using System.Runtime.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace SchemaSmith.IO;

public class YamlStringEnumConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type.IsEnum;

    public object ReadYaml(IParser parser, Type type)
    {
        var parsedEnum = parser.Consume<Scalar>();
        var serializableValues = type.GetMembers()
            .Where(m => m.GetCustomAttributes<EnumMemberAttribute>(true).Select(ema => ema.Value).Any())
            .Select(m => 
                new KeyValuePair<string, MemberInfo> (
                    m.GetCustomAttributes<EnumMemberAttribute>(true).Select(ema => ema.Value).FirstOrDefault()!, m))
            .Where(pa => !string.IsNullOrEmpty(pa.Key)).ToDictionary(pa => pa.Key, pa => pa.Value);
        if (!serializableValues.ContainsKey(parsedEnum.Value))
        {
            throw new YamlException(parsedEnum.Start, parsedEnum.End, $"Value '{parsedEnum.Value}' not found in enum '{type.Name}'");
        }

        return Enum.Parse(type, serializableValues[parsedEnum.Value].Name);
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type)
    {
        var enumMember = type.GetMember(value?.ToString() ?? throw new ArgumentNullException()).FirstOrDefault();
        var yamlValue = enumMember?.GetCustomAttributes<EnumMemberAttribute>(true).Select(ema => ema.Value).FirstOrDefault() ?? value.ToString();
        emitter.Emit(new Scalar(yamlValue!));
    }
}