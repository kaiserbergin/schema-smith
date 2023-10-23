using Graphr.Neo4j.Configuration;
using Graphr.Neo4j.Driver;
using Graphr.Neo4j.Graphr;
using Graphr.Neo4j.QueryExecution;
using Neo4j.Driver;
using SchemaSmith.Domain.Interfaces;
using SchemaSmith.Neo4j.Domain.Dto;
using SchemaSmith.Neo4j.Infrastructure.Queries.Provider;
using Index = SchemaSmith.Neo4j.Domain.Dto.Index;

namespace SchemaSmith.Neo4j.Infrastructure.Introspection;

public class NeoSchemaRepository : IIntrospectionRepository<ServerSchema>
{
    private readonly INeoGraphr _neoGraphr;
    private readonly NeoDriverConfigurationSettings _settings;

    private Dictionary<string, ConstraintType> _constraintTypeMap = new Dictionary<string, ConstraintType>
    {
        { "RELATIONSHIP_PROPERTY_EXISTENCE", ConstraintType.Existence },
        { "NODE_KEY", ConstraintType.NodeKey },
        { "NODE_PROPERTY_EXISTENCE", ConstraintType.Existence },
        { "UNIQUENESS", ConstraintType.Unique }
    };

    private Dictionary<string, IndexType> _indexTypeMap = new Dictionary<string, IndexType>
    {
        { "BTREE", IndexType.BTree },
        { "POINT", IndexType.Point },
        { "RANGE", IndexType.Range },
        { "TEXT", IndexType.Text },
    };

    public NeoSchemaRepository(NeoDriverConfigurationSettings settings)
    {
        var driverProvider = new DriverProvider(settings);
        var queryExecutor = new QueryExecutor(driverProvider, settings);
        _neoGraphr = new NeoGraphr(queryExecutor);
        _settings = settings;
    }

    public async Task<ServerSchema> GetServerSchemaAsync()
    {
        var nodesAndRelationships = await GetDatabaseEntitiesAsync();

        return new ServerSchema
        {
            ServerUrl = _settings.Url,
            Graphs = new List<GraphSchema>()
            {
                new GraphSchema
                {
                    Name = _settings.DatabaseName,
                    Nodes = nodesAndRelationships.Nodes,
                    Relationships = nodesAndRelationships.Relationships,
                    Constraints = await GetConstraintsAsync(),
                    Indexes = await GetIndexesAsync()
                }
            }
        };
    }

    public async Task<List<Constraint>> GetConstraintsAsync()
    {
        var records = await _neoGraphr
            .ReadAsync(QueryProvider.ShowConstraints);

        return records.Select(record => new Constraint
        {
            Name = record["name"].As<string>(),
            Type = _constraintTypeMap[record["type"].As<string>()],
            Entity = new Entity
            {
                Type = record["entityType"].As<string>() == "NODE" ? EntityType.Node : EntityType.Relationship,
                Id = record["labelsOrTypes"].As<List<string>>().FirstOrDefault(),
                Properties = record["properties"].As<List<string>>()
            }
        }).ToList();
    }

    private async Task<List<Index>> GetIndexesAsync()
    {
        var records = await _neoGraphr.ReadAsync(QueryProvider.ShowIndexes);

        return records
            .Where(record => _indexTypeMap.TryGetValue(record["type"].As<string>(), out _))
            .Select(record => new Index
            {
                Name = record["name"].As<string>(),
                Type = _indexTypeMap[record["type"].As<string>()],
                Entity = new Entity
                {
                    Type = record["entityType"].As<string>() == "NODE" ? EntityType.Node : EntityType.Relationship,
                    Id = record["labelsOrTypes"].As<List<string>>().FirstOrDefault(),
                    Properties = record["properties"].As<List<string>>()
                }
            }).ToList();
    }
    
    public async Task<string> GetVersionAsync()
    {
        var records = await _neoGraphr.ReadAsync(QueryProvider.GetVersions);

        return records
            .Select(record => record["version"].As<string>())
            .Max();
    }

    public async Task<(List<Node> Nodes, List<Relationship> Relationships)> GetDatabaseEntitiesAsync()
    {
        var nodes = new List<Node>();
        var relationships = new Dictionary<string, Relationship>();

        var records = await _neoGraphr.ReadAsync(QueryProvider.ShowMetaSchema);

        foreach (var record in records)
        {
            var resultMap = record[0].As<Dictionary<string, object?>>();

            foreach (var resultKvp in resultMap)
            {
                var entityId = resultKvp.Key;
                var entity = resultKvp.Value.As<Dictionary<string, object?>>();
                var entityType = entity["type"].As<string>();
                
                if (entityType == "node")
                {
                    nodes.Add(new Node
                    {
                        Label = entityId,
                        Properties = entity["properties"]
                            .As<Dictionary<string, object?>>()
                            .Select(propertyKvp =>
                                {
                                    var props = propertyKvp.Value.As<Dictionary<string, object?>>();

                                    return new Property
                                    {
                                        Name = propertyKvp.Key,
                                        Type = ConvertToDataType(props["type"].As<string>())
                                    };
                                }
                            ).ToList()
                    });

                    var relationshipMap = entity["relationships"].As<Dictionary<string, object?>>();

                    foreach (var relKvp in relationshipMap)
                    {
                        Relationship rel;

                        if (relationships.ContainsKey(relKvp.Key))
                            rel = relationships[relKvp.Key];
                        else
                        {
                            rel = new Relationship
                            {
                                Type = relKvp.Key,
                            };

                            relationships.Add(relKvp.Key, rel);
                        }

                        var relProps = relKvp.Value.As<Dictionary<string, object?>>();

                        var labels = relProps["labels"].As<List<string>>();

                        foreach (var label in labels)
                        {
                            var direction = relProps["direction"].As<string>();
                            var connectionString = direction == "in"
                                ? $"{label}->{entityId}"
                                : $"{entityId}->{label}";

                            rel.Connections.Add(connectionString);
                        }
                    }
                }
                else if (entityType == "relationship")
                {
                    Relationship rel;

                    if (relationships.ContainsKey(entityId))
                        rel = relationships[entityId];
                    else
                    {
                        rel = new Relationship
                        {
                            Type = entityId,
                        };
                    
                        relationships.Add(entityId, rel);
                    }

                    var properties = entity["properties"]
                        .As<Dictionary<string, object?>>()
                        .Select(propertyKvp =>
                            {
                                var props = propertyKvp.Value.As<Dictionary<string, object?>>();

                                return new Property
                                {
                                    Name = propertyKvp.Key,
                                    Type = ConvertToDataType(props["type"].As<string>())
                                };
                            }
                        ).ToList();

                    rel.Properties.AddRange(properties);
                }
            }
        }

        return (nodes, relationships.Values.ToList());
    }

    private NeoDataType ConvertToDataType(string recordDataType) =>
        recordDataType switch
        {
            "LOCAL_DATE_TIME" => NeoDataType.LocalDateTime,
            "DATE_TIME" => NeoDataType.DateTime,
            "STRING" => NeoDataType.String,
            "LIST" => NeoDataType.ListString,
            "FLOAT" => NeoDataType.Float,
            "DATE" => NeoDataType.Date,
            "POINT" => NeoDataType.Point,
            "INTEGER" => NeoDataType.Integer,
            "DURATION" => NeoDataType.Duration,
            "LOCAL_TIME" => NeoDataType.LocalTime,
            "BOOLEAN" => NeoDataType.Boolean,
            "TIME" => NeoDataType.Time,
            _ => NeoDataType.String
        };
}