using System.Runtime.Serialization;

namespace SchemaSmith.Domain;

internal class Server
{
    /// <summary>
    /// Url for you Neo4j Database Server
    /// </summary>
    internal string ServerUrl { get; init; } = null!;

    /// <summary>
    /// List of graphs associated to your server
    /// </summary>
    internal IEnumerable<Graph> Graphs { get; init; } = Enumerable.Empty<Graph>();
}

internal class Graph
{
    /// <summary>
    /// Graph name
    /// </summary>
    internal string Name { get; init; } = null!;

    /// <summary>
    /// List of node labels with associated properties
    /// </summary>
    internal IEnumerable<Node> Nodes { get; init; } = Enumerable.Empty<Node>();

    /// <summary>
    /// List of relationships in your graph
    /// </summary>
    internal IEnumerable<Relationship> Relationships { get; init; } = Enumerable.Empty<Relationship>();

    /// <summary>
    /// Constraints on your graph
    /// </summary>
    internal IEnumerable<Constraint> Constraints { get; init; } = Enumerable.Empty<Constraint>();

    /// <summary>
    /// Indexes on your graph
    /// </summary>
    internal IEnumerable<Index> Indexes { get; init; } = Enumerable.Empty<Index>();
}

internal class Node
{
    /// <summary>
    /// Label for a given node
    /// </summary>
    internal string Label { get; init; } = null!;

    /// <summary>
    /// List of properties associated with the label
    /// </summary>
    internal IEnumerable<Property> Properties { get; init; } = Enumerable.Empty<Property>();
}

internal class Relationship
{
    /// <summary>
    /// Relationship name, used to create / define relationships
    /// </summary>
    internal string Type { get; init; } = null!;

    /// <summary>
    /// List of properties associated with the label
    /// </summary>
    internal IEnumerable<Property> Properties { get; init; } = Enumerable.Empty<Property>();

    /// <summary>
    /// List of connections with the format: LabelName-&gt;OtherLabelName
    /// </summary>
    internal IEnumerable<string> Connections { get; init; } = Enumerable.Empty<string>();
}

internal class Constraint
{
    /// <summary>
    /// Constraint name used for your graph
    /// </summary>
    internal string Name { get; init; } = null!;

    /// <summary>
    /// Type of constraint
    /// </summary>
    internal ConstraintType Type { get; init; }

    /// <summary>
    /// Entity that the constraint will be created for.
    /// </summary>
    internal Entity Entity { get; init; } = new();
}

internal enum ConstraintType
{
    [EnumMember(Value = @"node-key")]
    NodeKey,
    [EnumMember(Value = @"unique")]
    Unique,
    [EnumMember(Value = @"existence")]
    Existence
}

internal class Index
{
    /// <summary>
    /// Index name used for your graph
    /// </summary>
    internal string Name { get; init; } = null!;

    /// <summary>
    /// Type of index
    /// </summary>
    internal IndexType Type { get; init; }

    /// <summary>
    /// Entity that the constraint will be created for.
    /// </summary>
    internal Entity Entity { get; set; } = new();
}

internal enum IndexType
{
    [EnumMember(Value = @"b-tree")]
    BTree,
    [EnumMember(Value = @"text")]
    Text,
    [EnumMember(Value = @"point")]
    Point,
    [EnumMember(Value = @"range")]
    Range,
}

/// <summary>
/// Property on an entity.
/// </summary>
internal class Property
{
    /// <summary>
    /// Property name.
    /// </summary>
    internal string Name { get; init; } = null!;

    /// <summary>
    /// Property data type.
    /// </summary>
    internal PropertyType Type { get; init; } = PropertyType.String;
}

internal class Entity
{
    internal EntityType Type { get; init; }

    /// <summary>
    /// Entity id. (Label for Nodes, Type for Relationships)
    /// </summary>
    internal string Id { get; init; } = null!;

    /// <summary>
    /// Properties used for the constraint.
    /// </summary>
    internal IEnumerable<string> Properties { get; init; } = Enumerable.Empty<string>();
}

internal enum EntityType
{
    [EnumMember(Value = @"node")]
    Node,
    [EnumMember(Value = @"relationship")]
    Relationship
}

internal enum PropertyType
{
    [EnumMember(Value = @"string")]
    String = 0,


    [EnumMember(Value = @"integer")]
    Integer = 1,


    [EnumMember(Value = @"float")]
    Float = 2,


    [EnumMember(Value = @"boolean")]
    Boolean = 3,


    [EnumMember(Value = @"point")]
    Point = 4,


    [EnumMember(Value = @"date")]
    Date = 5,


    [EnumMember(Value = @"time")]
    Time = 6,


    [EnumMember(Value = @"localTime")]
    LocalTime = 7,


    [EnumMember(Value = @"dateTime")]
    DateTime = 8,


    [EnumMember(Value = @"localDateTime")]
    LocalDateTime = 9,


    [EnumMember(Value = @"duration")]
    Duration = 10,


    [EnumMember(Value = @"list(string)")]
    ListString = 11,


    [EnumMember(Value = @"list(integer)")]
    ListInteger = 12,


    [EnumMember(Value = @"list(float)")]
    ListFloat = 13,


    [EnumMember(Value = @"list(boolean)")]
    ListBoolean = 14,


    [EnumMember(Value = @"list(point)")]
    ListPoint = 15,


    [EnumMember(Value = @"list(date)")]
    ListDate = 16,


    [EnumMember(Value = @"list(time)")]
    ListTime = 17,


    [EnumMember(Value = @"list(localTime)")]
    ListLocalTime = 18,


    [EnumMember(Value = @"list(dateTime)")]
    ListDateTime = 19,


    [EnumMember(Value = @"list(localDateTime)")]
    ListLocalDateTime = 20,


    [EnumMember(Value = @"list(duration)")]
    ListDuration = 21,
}