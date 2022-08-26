using System.Text;
using SchemaSmith.Domain;

namespace SchemaSmith.CypherGenerator;

internal static class NodeExtensions
{
    internal static string GenerateCypher(this Node node)
    {
        var sb = new StringBuilder();

        sb.Append($"CREATE (n:{node.Label}:{Constants.SCHEMA_SMITH_LABEL_NAME})");

        foreach (var property in node.Properties)
        {
            sb.Append($"\nSET n.{ property.Name } = { property.GenerateDefaultPropertyValue() }");
        }

        sb.Append(';');

        return sb.ToString();
    }
}