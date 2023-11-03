using System.Text;
using SchemaSmith.Neo4j.Domain.Dto;

namespace SchemaSmith.Neo4j.Core.ScriptGeneration.ExtensionMethods;

public static class NodeExtensions
{
    public static string GenerateCypher(this Node node)
    {
        var sb = new StringBuilder();

        sb.Append($"CREATE (n:{node.Label}:{SchemaSmithConstants.SCHEMA_SMITH_ENTITY_IDENTIFIER})");

        foreach (var property in node.Properties)
        {
            sb.Append($"\nSET n.{ property.Name } = { property.GenerateDefaultPropertyValue() }");
        }

        sb.Append(';');

        return sb.ToString();
    }
}