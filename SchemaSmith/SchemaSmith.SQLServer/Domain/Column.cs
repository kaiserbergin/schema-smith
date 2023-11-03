namespace SchemaSmith.SQLServer.Domain;

public class Column
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string DataType { get; set; } = null!;
    public string? Source { get; set; }
    public string? Example { get; set; }
    public List<string> Constraints { get; set; } = new();
}