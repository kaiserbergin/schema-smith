namespace SchemaSmith.SQLServer.Domain;

public class Schema
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public List<Table> Tables { get; set; } = new();
}