using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SchemaSmith.Utilities;

internal static class EnumExtensions
{
    internal static string? GetEnumDisplayName(this Enum enumValue) =>
        enumValue.GetType()
            .GetMember(enumValue.ToString())?
            .First()
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName();
}