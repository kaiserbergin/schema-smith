using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class PropertyScalarType : ScalarNodeValidationDefinition
{
    public PropertyScalarType()
    {
        AllowedEnumeration = new HashSet<string>
        {
            "string",
            "integer",
            "float",
            "boolean",
            "point",
            "date",
            "time",
            "localTime",
            "dateTime",
            "localDateTime",
            "duration",
            "list(string)",
            "list(integer)",
            "list(float)",
            "list(boolean)",
            "list(point)",
            "list(date)",
            "list(time)",
            "list(localTime)",
            "list(dateTime)",
            "list(localDateTime)",
            "list(duration)"
        };
    }
}