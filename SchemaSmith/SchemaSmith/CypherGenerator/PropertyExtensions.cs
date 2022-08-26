using SchemaSmith.Domain;

namespace SchemaSmith.CypherGenerator;

internal static class PropertyExtensions
{
    private static readonly Dictionary<PropertyType, string> _defaultPropertyValues = new()
    {
        { PropertyType.String, "\"string\"" },
        { PropertyType.Integer, "0" },
        { PropertyType.Float, "1.0" },
        { PropertyType.Boolean, "false" },
        { PropertyType.Point, "point({ latitude: 0, longitude: 0 })" },
        { PropertyType.Date, "date()" },
        { PropertyType.Time, "time()" },
        { PropertyType.LocalTime, "localtime()" },
        { PropertyType.DateTime, "datetime()" },
        { PropertyType.LocalDateTime, "localdatetime()" },
        { PropertyType.Duration, "duration('P14DT16H12M')" },
        { PropertyType.ListString, "[ \"string\" ]" },
        { PropertyType.ListInteger, "[ 0 ]" },
        { PropertyType.ListFloat, "[ 1.0 ]" },
        { PropertyType.ListBoolean, "[ false ]" },
        { PropertyType.ListPoint, "[ point({ latitude: 0, longitude: 0 }) ]" },
        { PropertyType.ListDate, "[ date() ]" },
        { PropertyType.ListTime, "[ time() ]" },
        { PropertyType.ListLocalTime, "[ localtime() ]" },
        { PropertyType.ListDateTime, "[ datetime() ]" },
        { PropertyType.ListLocalDateTime, "[ localdatetime() ]" },
        { PropertyType.ListDuration, "[ duration('P14DT16H12M') ]" }
    };

    internal static string GenerateDefaultPropertyValue(this Property property) =>
        _defaultPropertyValues[property.Type];
}