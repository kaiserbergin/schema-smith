using System.Globalization;
using SchemaSmith.Domain;
using SchemaSmith.Neo4j.Domain.Dto;

namespace SchemaSmith.CypherStatementExtensions;

internal static class PropertyExtensions
{
    private static string _defaultGuid => Guid.NewGuid().ToString();
    private static string _defaultBool => false.ToString().ToLowerInvariant();
    private static string _defaultInt => (-9999).ToString();
    private static string _defaultFloat => (-99.99).ToString(CultureInfo.InvariantCulture);

    private static readonly Dictionary<NeoDataType, Func<string>> _defaultPropertyValues = new()
    {
        { NeoDataType.String, () => $"\"{_defaultGuid}\"" },
        { NeoDataType.Integer, () => _defaultInt },
        { NeoDataType.Float, () => _defaultFloat },
        { NeoDataType.Boolean, () => _defaultBool },
        { NeoDataType.Point, () => "point({ latitude: 0, longitude: 0 })" },
        { NeoDataType.Date, () => "date()" },
        { NeoDataType.Time, () => "time()" },
        { NeoDataType.LocalTime, () => "localtime()" },
        { NeoDataType.DateTime, () => "datetime()" },
        { NeoDataType.LocalDateTime, () => "localdatetime()" },
        { NeoDataType.Duration, () => "duration('P14DT16H12M')" },
        { NeoDataType.ListString, () => $"[ \"{_defaultGuid}\" ]" },
        { NeoDataType.ListInteger, () => $"[ {_defaultInt} ]" },
        { NeoDataType.ListFloat, () => $"[ {_defaultFloat} ]" },
        { NeoDataType.ListBoolean, () => $"[ {_defaultBool}]" },
        { NeoDataType.ListPoint, () => "[ point({ latitude: 0, longitude: 0 }) ]" },
        { NeoDataType.ListDate, () => "[ date() ]" },
        { NeoDataType.ListTime, () => "[ time() ]" },
        { NeoDataType.ListLocalTime, () => "[ localtime() ]" },
        { NeoDataType.ListDateTime, () => "[ datetime() ]" },
        { NeoDataType.ListLocalDateTime, () => "[ localdatetime() ]" },
        { NeoDataType.ListDuration, () => "[ duration('P14DT16H12M') ]" }
    };

    internal static string GenerateDefaultPropertyValue(this Property property) =>
        _defaultPropertyValues[property.Type]();
}