using System.Reflection;
using SchemaSmith.CypherStatementExtensions;
using SchemaSmith.Domain;

namespace CodeGenerators.dotnet;

partial class GraphrGenerator
{
    private string _assemblyVersion;
    private ServerSchema _serverSchema;
    private Dictionary<string, List<(string SourceNode, string TargetNode)>> _relationshipTargets;

    public GraphrGenerator(ServerSchema serverSchema)
    {
        var assemblyName = typeof(ServerSchema).Assembly.GetName();
        _assemblyVersion = assemblyName.Version!.ToString();
        
        _serverSchema = serverSchema ?? throw new ArgumentNullException(nameof(serverSchema));
        
        AssignRelationshipTargets();
    }

    private string GetCLRType(NeoDataType neoDataType) =>
        neoDataType switch
        {
            // Okay, but this is easier than doing all the reflection. Change my mind.
            NeoDataType.String => "string",
            NeoDataType.ListString => "IEnumerable<string>",
            NeoDataType.Boolean => "bool",
            NeoDataType.ListBoolean => "IEnumerable<bool>",
            NeoDataType.Date => "Date",
            NeoDataType.ListDate => "IEnumerable<Date>",
            NeoDataType.Duration => "Duration",
            NeoDataType.ListDuration => "IEnumerable<Duration>",
            NeoDataType.Float => "float",
            NeoDataType.ListFloat => "IEnumerable<float>",
            NeoDataType.Integer => "long",
            NeoDataType.ListInteger => "IEnumerable<long>",
            NeoDataType.Point => "Point",
            NeoDataType.ListPoint => "IEnumerable<Point>",
            NeoDataType.Time => "OffsetTime",
            NeoDataType.ListTime => "IEnumerable<OffsetTime>",
            NeoDataType.DateTime => "DateTime",
            NeoDataType.ListDateTime => "IEnumerable<DateTime>",
            NeoDataType.LocalDateTime => "LocalDateTime",
            NeoDataType.ListLocalDateTime => "IEnumerable<LocalDateTime>",
            NeoDataType.LocalTime => "LocalTime",
            NeoDataType.ListLocalTime => "IEnumerable<LocalTime>",
        };

    private void GetRelationshipInfoForLabel(string label)
    {
        
    }

    private void AssignRelationshipTargets()
    {
        foreach (var relationship in _serverSchema.Graphs.FirstOrDefault()?.Relationships ?? new List<Relationship>())
        {
            _relationshipTargets.TryAdd(relationship.Type, new List<(string SourceNode, string TargetNode)>());

            foreach (var connection in relationship.Connections)
            {
                var relationshipInfo = RelationshipExtensions.GetRelationshipInfo(connection);

                switch (relationshipInfo.direction)
                {
                    case RelationshipExtensions.OUTGOING_RELATIONSHIP:
                        _relationshipTargets[relationship.Type].Add(new (relationshipInfo.firstLabel, relationshipInfo.secondLabel));
                        break;
                    case RelationshipExtensions.INCOMING_RELATIONSHIP:
                        _relationshipTargets[relationship.Type].Add(new (relationshipInfo.secondLabel, relationshipInfo.firstLabel));
                        break;
                    case RelationshipExtensions.DIRECTIONLESS_RELATIONSHIP:
                        _relationshipTargets[relationship.Type].Add(new (relationshipInfo.firstLabel, relationshipInfo.secondLabel));
                        _relationshipTargets[relationship.Type].Add(new (relationshipInfo.secondLabel, relationshipInfo.firstLabel));
                        break;
                    default:
                        throw new ArgumentException($"Proper direction not found on relationship {relationship.Type} for connection: {connection}");
                }
            }
        }
    }

    private HashSet<string> GetRelationshipTargets(Relationship relationship)
    {
        var targets = new HashSet<string>();

        foreach (var connection in relationship.Connections)
        {
            var relationshipInfo = RelationshipExtensions.GetRelationshipInfo(connection);

            switch (relationshipInfo.direction)
            {
                case RelationshipExtensions.OUTGOING_RELATIONSHIP:
                    targets.Add(relationshipInfo.secondLabel);
                    break;
                case RelationshipExtensions.INCOMING_RELATIONSHIP:
                    targets.Add(relationshipInfo.firstLabel);
                    break;
                case RelationshipExtensions.DIRECTIONLESS_RELATIONSHIP:
                    targets.Add(relationshipInfo.firstLabel);
                    targets.Add(relationshipInfo.secondLabel);
                    break;
                default:
                    throw new ArgumentException($"Proper direction not found on relationship {relationship.Type} for connection: {connection}");
            }
        }

        return targets;
    } 
}