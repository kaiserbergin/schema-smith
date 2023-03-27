using Graphr.Neo4j.Configuration;
using Graphr.Neo4j.Driver;
using Graphr.Neo4j.Graphr;
using Graphr.Neo4j.QueryExecution;
using Neo4j.Driver;
using SchemaSmith.Domain;
using SchemaSmith.Queries.Provider;
using Index = SchemaSmith.Domain.Index;

namespace SchemaSmith.DbIntrospection;

public class NeoSchemaRepository : INeoSchemaRepository
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

    public ServerSchema GetServerSchema()
    {
        var nodesAndRelationships = GetDatabaseEntities();

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
                    Constraints = GetConstraints(),
                    Indexes = GetIndexes()
                }
            }
        };
    }

    public List<Constraint> GetConstraints()
    {
        var records = _neoGraphr.ReadAsync(QueryProvider.ShowConstraints).GetAwaiter().GetResult();

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

    public List<Index> GetIndexes()
    {
        var records = _neoGraphr.ReadAsync(QueryProvider.ShowIndexes).GetAwaiter().GetResult();

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
    
    public string GetVersion()
    {
        var records = _neoGraphr.ReadAsync(QueryProvider.GetVersions).GetAwaiter().GetResult();

        return records
            .Select(record => record["version"].As<string>())
            .Max();

        // return records
        //     .Where(record => _indexTypeMap.TryGetValue(record["type"].As<string>(), out _))
        //     .Select(record => new Index
        //     {
        //         Name = record["name"].As<string>(),
        //         Type = _indexTypeMap[record["type"].As<string>()],
        //         Entity = new Entity
        //         {
        //             Type = record["entityType"].As<string>() == "NODE" ? EntityType.Node : EntityType.Relationship,
        //             Id = record["labelsOrTypes"].As<List<string>>().FirstOrDefault(),
        //             Properties = record["properties"].As<List<string>>()
        //         }
        //     }).ToList();
    }

    public (List<Node> Nodes, List<Relationship> Relationships) GetDatabaseEntities()
    {
        var nodes = new List<Node>();
        var relationships = new Dictionary<string, Relationship>();

        var records = _neoGraphr.ReadAsync(QueryProvider.ShowMetaSchema).GetAwaiter().GetResult();

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
                            var connectionString = relProps["direction"].As<string>() == "in"
                                ? $"{entityId}<-{label}"
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