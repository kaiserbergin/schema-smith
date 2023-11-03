namespace SchemaSmith.SQLServer.Domain;

public class Index
{
    public string Name { get; set; } = null!;
    public bool IsUnique { get; set; } = false;
    public List<string> Columns { get; set; } = new();
}