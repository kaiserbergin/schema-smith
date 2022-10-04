using SchemaSmith.Domain;

namespace SchemaSmith.CypherStatementExtensions;

internal static class PropertyExtensions
{
    private static readonly Dictionary<NeoDataType, string> _defaultPropertyValues = new()
    {
        { NeoDataType.String, "\"string\"" },
        { NeoDataType.Integer, "0" },
        { NeoDataType.Float, "1.0" },
        { NeoDataType.Boolean, "false" },
        { NeoDataType.Point, "point({ latitude: 0, longitude: 0 })" },
        { NeoDataType.Date, "date()" },
        { NeoDataType.Time, "time()" },
        { NeoDataType.LocalTime, "localtime()" },
        { NeoDataType.DateTime, "datetime()" },
        { NeoDataType.LocalDateTime, "localdatetime()" },
        { NeoDataType.Duration, "duration('P14DT16H12M')" },
        { NeoDataType.ListString, "[ \"string\" ]" },
        { NeoDataType.ListInteger, "[ 0 ]" },
        { NeoDataType.ListFloat, "[ 1.0 ]" },
        { NeoDataType.ListBoolean, "[ false ]" },
        { NeoDataType.ListPoint, "[ point({ latitude: 0, longitude: 0 }) ]" },
        { NeoDataType.ListDate, "[ date() ]" },
        { NeoDataType.ListTime, "[ time() ]" },
        { NeoDataType.ListLocalTime, "[ localtime() ]" },
        { NeoDataType.ListDateTime, "[ datetime() ]" },
        { NeoDataType.ListLocalDateTime, "[ localdatetime() ]" },
        { NeoDataType.ListDuration, "[ duration('P14DT16H12M') ]" }
    };

    internal static string GenerateDefaultPropertyValue(this Property property) =>
        _defaultPropertyValues[property.Type];
}