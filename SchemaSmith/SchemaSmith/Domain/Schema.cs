using System.Runtime.Serialization;

namespace SchemaSmith.Domain;

public class ServerSchema
{
    /// <summary>
    /// Url for you Neo4j Database Server
    /// </summary>
    public string ServerUrl { get; init; } = null!;

    /// <summary>
    /// List of graphs associated to your server
    /// </summary>
    public List<GraphSchema> Graphs { get; init; } = new List<GraphSchema>();
}

public class GraphSchema
{
    /// <summary>
    /// Graph name
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// List of node labels with associated properties
    /// </summary>
    public List<Node> Nodes { get; init; } = new List<Node>();

    /// <summary>
    /// List of relationships in your graph
    /// </summary>
    public List<Relationship> Relationships { get; init; } = new List<Relationship>();

    /// <summary>
    /// Constraints on your graph
    /// </summary>
    public List<Constraint> Constraints { get; init; } = new List<Constraint>();

    /// <summary>
    /// Indexes on your graph
    /// </summary>
    public List<Index> Indexes { get; init; } = new List<Index>();
}



public class Node
{
    /// <summary>
    /// Label for a given node
    /// </summary>
    public string Label { get; init; } = null!;

    /// <summary>
    /// List of properties associated with the label
    /// </summary>
    public List<Property> Properties { get; init; } = new List<Property>();
}

public class Relationship
{
    /// <summary>
    /// Relationship name, used to create / define relationships
    /// </summary>
    public string Type { get; init; } = null!;

    /// <summary>
    /// List of properties associated with the label
    /// </summary>
    public List<Property> Properties { get; init; } = new List<Property>();

    /// <summary>
    /// List of connections with the format: LabelName-&gt;OtherLabelName
    /// </summary>
    public List<string> Connections { get; init; } = new List<string>();
}

public class Constraint
{
    /// <summary>
    /// Constraint name used for your graph
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Type of constraint
    /// </summary>
    public ConstraintType Type { get; init; }

    /// <summary>
    /// Entity that the constraint will be created for.
    /// </summary>
    public Entity Entity { get; init; } = new();
}

public enum ConstraintType
{
    [EnumMember(Value = @"node-key")]
    NodeKey = 1,
    [EnumMember(Value = @"unique")]
    Unique = 2,
    [EnumMember(Value = @"existence")]
    Existence = 3
}

public class Index
{
    /// <summary>
    /// Index name used for your graph
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Type of index
    /// </summary>
    public IndexType Type { get; init; }

    /// <summary>
    /// Entity that the constraint will be created for.
    /// </summary>
    public Entity Entity { get; set; } = new();
}

public enum IndexType
{
    [EnumMember(Value = @"b-tree")]
    BTree = 1,
    [EnumMember(Value = @"text")]
    Text = 2,
    [EnumMember(Value = @"point")]
    Point = 3,
    [EnumMember(Value = @"range")]
    Range = 4,
}

/// <summary>
/// Property on an entity.
/// </summary>
public class Property
{
    /// <summary>
    /// Property name.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Property data type.
    /// </summary>
    public NeoDataType Type { get; init; } = NeoDataType.String;
}

public class Entity
{
    public EntityType Type { get; init; }

    /// <summary>
    /// Entity id. (Label for Nodes, Type for Relationships)
    /// </summary>
    public string Id { get; init; } = null!;

    /// <summary>
    /// Properties used for the constraint.
    /// </summary>
    public List<string> Properties { get; init; } = new List<string>();
}

public enum EntityType
{
    [EnumMember(Value = @"node")]
    Node = 1,
    [EnumMember(Value = @"relationship")]
    Relationship = 2
}

public enum NeoDataType
{
    [EnumMember(Value = @"string")]
    String = 1,


    [EnumMember(Value = @"integer")]
    Integer = 2,


    [EnumMember(Value = @"float")]
    Float = 3,


    [EnumMember(Value = @"boolean")]
    Boolean = 4,


    [EnumMember(Value = @"point")]
    Point = 5,


    [EnumMember(Value = @"date")]
    Date = 6,


    [EnumMember(Value = @"time")]
    Time = 7,


    [EnumMember(Value = @"localTime")]
    LocalTime = 8,


    [EnumMember(Value = @"dateTime")]
    DateTime = 9,


    [EnumMember(Value = @"localDateTime")]
    LocalDateTime = 10,


    [EnumMember(Value = @"duration")]
    Duration = 11,


    [EnumMember(Value = @"list(string)")]
    ListString = 12,


    [EnumMember(Value = @"list(integer)")]
    ListInteger = 13,


    [EnumMember(Value = @"list(float)")]
    ListFloat = 14,


    [EnumMember(Value = @"list(boolean)")]
    ListBoolean = 15,


    [EnumMember(Value = @"list(point)")]
    ListPoint = 16,


    [EnumMember(Value = @"list(date)")]
    ListDate = 17,


    [EnumMember(Value = @"list(time)")]
    ListTime = 18,


    [EnumMember(Value = @"list(localTime)")]
    ListLocalTime = 19,


    [EnumMember(Value = @"list(dateTime)")]
    ListDateTime = 20,


    [EnumMember(Value = @"list(localDateTime)")]
    ListLocalDateTime = 21,


    [EnumMember(Value = @"list(duration)")]
    ListDuration = 22
}