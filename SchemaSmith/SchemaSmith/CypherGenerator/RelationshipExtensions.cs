using System.Text;
using SchemaSmith.Domain;

namespace SchemaSmith.CypherGenerator;

internal static class RelationshipExtensions
{
    private const string INCOMING_RELATIONSHIP = nameof(INCOMING_RELATIONSHIP);
    private const string OUTGOING_RELATIONSHIP = nameof(OUTGOING_RELATIONSHIP);
    private const string DIRECTIONLESS_RELATIONSHIP = nameof(DIRECTIONLESS_RELATIONSHIP);

    internal static IEnumerable<string> GenerateCypher(this Relationship relationship)
    {
        var cypherStatements = new List<string>();
        
        foreach (var connection in relationship.Connections)
        {
            var relationshipInfo = GetRelationshipInfo(connection);
            var sb = new StringBuilder();

            sb.Append($"CREATE (n1:{relationshipInfo.firstLabel}:{SchemaSmithConstants.SCHEMA_SMITH_ENTITY_IDENTIFIER})");
            sb.Append(relationshipInfo.direction == OUTGOING_RELATIONSHIP ? '-' : "<-");
            sb.Append($"[r:{relationship.Type}]");
            sb.Append(relationshipInfo.direction == OUTGOING_RELATIONSHIP ? "->" : '-');
            sb.Append($"(n2:{relationshipInfo.secondLabel}:{SchemaSmithConstants.SCHEMA_SMITH_ENTITY_IDENTIFIER})");
            
            foreach (var property in relationship.Properties)
            {
                sb.Append($"\nSET r.{ property.Name } = { property.GenerateDefaultPropertyValue() }");
            }

            sb.Append(';');
            
            cypherStatements.Add(sb.ToString());
        }

        return cypherStatements;
    }

    private static (string firstLabel, string direction, string secondLabel) GetRelationshipInfo(string connectionStr)
    {
        var dashPosition = connectionStr.IndexOf("-", StringComparison.InvariantCultureIgnoreCase);
        var direction = connectionStr.IndexOf('>') > -1 ? OUTGOING_RELATIONSHIP
            : connectionStr.IndexOf('<') > -1 ? INCOMING_RELATIONSHIP : DIRECTIONLESS_RELATIONSHIP;

        var stopPosition = direction == INCOMING_RELATIONSHIP ? dashPosition - 1 : dashPosition;

        return (
            connectionStr[..stopPosition],
            connectionStr.IndexOf('>') > -1 ? OUTGOING_RELATIONSHIP : INCOMING_RELATIONSHIP,
            connectionStr[(stopPosition + 2)..]
        );
    }
}