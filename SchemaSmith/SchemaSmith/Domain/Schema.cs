namespace SchemaSmith.Domain;

public class Server
{
    /// <summary>
    /// Url for you Neo4j Database Server
    /// </summary>
    public string ServerUrl { get; init; } = null!;

    /// <summary>
    /// List of graphs associated to your server
    /// </summary>
    public IEnumerable<Graph> Graphs { get; init; } = Enumerable.Empty<Graph>();
}

public class Graph
{
    /// <summary>
    /// Graph name
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// List of node labels with associated properties
    /// </summary>
    public IEnumerable<Node> Nodes { get; init; } = Enumerable.Empty<Node>();

    /// <summary>
    /// List of relationships in your graph
    /// </summary>
    public IEnumerable<Relationship> Relationships { get; init; } = Enumerable.Empty<Relationship>();

    /// <summary>
    /// Constraints on your graph
    /// </summary>
    public IEnumerable<Constraint> Constraints { get; init; } = Enumerable.Empty<Constraint>();

    /// <summary>
    /// Indexes on your graph
    /// </summary>
    public IEnumerable<Index> Indexes { get; init; } = Enumerable.Empty<Index>();
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
    public IEnumerable<Property> Properties { get; init; } = Enumerable.Empty<Property>();
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
    public IEnumerable<Property> Properties { get; init; } = Enumerable.Empty<Property>();

    /// <summary>
    /// List of connections with the format: LabelName-&gt;OtherLabelName
    /// </summary>
    public IEnumerable<string> Connections { get; init; } = Enumerable.Empty<string>();
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
    [System.Runtime.Serialization.EnumMember(Value = @"node-key")]
    NodeKey,
    [System.Runtime.Serialization.EnumMember(Value = @"unique")]
    Unique,
    [System.Runtime.Serialization.EnumMember(Value = @"existence")]
    Existence
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
    public string Type { get; init; } = null!;

    /// <summary>
    /// Entity that the constraint will be created for.
    /// </summary>
    public Entity Entity { get; set; } = new();
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
    public PropertiesType Type { get; init; } = PropertiesType.String;
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
    public IEnumerable<string> Properties { get; init; } = Enumerable.Empty<string>();
}

public enum EntityType
{
    [System.Runtime.Serialization.EnumMember(Value = @"node")]
    Node,
    [System.Runtime.Serialization.EnumMember(Value = @"relationship")]
    Relationship
}

public enum PropertiesType
{
    [System.Runtime.Serialization.EnumMember(Value = @"string")]
    String = 0,


    [System.Runtime.Serialization.EnumMember(Value = @"integer")]
    Integer = 1,


    [System.Runtime.Serialization.EnumMember(Value = @"float")]
    Float = 2,


    [System.Runtime.Serialization.EnumMember(Value = @"boolean")]
    Boolean = 3,


    [System.Runtime.Serialization.EnumMember(Value = @"point")]
    Point = 4,


    [System.Runtime.Serialization.EnumMember(Value = @"date")]
    Date = 5,


    [System.Runtime.Serialization.EnumMember(Value = @"time")]
    Time = 6,


    [System.Runtime.Serialization.EnumMember(Value = @"localTime")]
    LocalTime = 7,


    [System.Runtime.Serialization.EnumMember(Value = @"dateTime")]
    DateTime = 8,


    [System.Runtime.Serialization.EnumMember(Value = @"localDateTime")]
    LocalDateTime = 9,


    [System.Runtime.Serialization.EnumMember(Value = @"duration")]
    Duration = 10,


    [System.Runtime.Serialization.EnumMember(Value = @"list(string)")]
    ListString = 11,


    [System.Runtime.Serialization.EnumMember(Value = @"list(integer)")]
    ListInteger = 12,


    [System.Runtime.Serialization.EnumMember(Value = @"list(float)")]
    ListFloat = 13,


    [System.Runtime.Serialization.EnumMember(Value = @"list(boolean)")]
    ListBoolean = 14,


    [System.Runtime.Serialization.EnumMember(Value = @"list(point)")]
    ListPoint = 15,


    [System.Runtime.Serialization.EnumMember(Value = @"list(date)")]
    ListDate = 16,


    [System.Runtime.Serialization.EnumMember(Value = @"list(time)")]
    ListTime = 17,


    [System.Runtime.Serialization.EnumMember(Value = @"list(localTime)")]
    ListLocalTime = 18,


    [System.Runtime.Serialization.EnumMember(Value = @"list(dateTime)")]
    ListDateTime = 19,


    [System.Runtime.Serialization.EnumMember(Value = @"list(localDateTime)")]
    ListLocalDateTime = 20,


    [System.Runtime.Serialization.EnumMember(Value = @"list(duration)")]
    ListDuration = 21,
}