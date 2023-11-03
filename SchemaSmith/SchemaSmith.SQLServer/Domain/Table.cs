namespace SchemaSmith.SQLServer.Domain;

public class Table
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public List<Column> Columns { get; set; } = new();
    public PrimaryKey? PrimaryKey { get; set; }
    public List<ForeignKey> ForeignKeys { get; set; } = new();
    public List<Index> Indexes { get; set; } = new();
    public List<CustomScript> Scripts { get; set; } = new();
}