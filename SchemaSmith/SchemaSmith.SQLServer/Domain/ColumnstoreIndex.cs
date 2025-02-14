namespace SchemaSmith.SQLServer.Domain;

public class ColumnstoreIndex
{
    public string Name { get; set; } = null!;
    public bool IsClustered { get; set; } = true;
    public List<string> Columns { get; set; } = new();
    public List<string> Order { get; set; } = new();
    public string? AdditionalClauses { get; set; }
}