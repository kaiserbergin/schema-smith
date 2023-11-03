namespace SchemaSmith.SQLServer.Domain;

public class PrimaryKey
{
    public string Name { get; set; } = null!;
    public List<string> Columns { get; set; } = new();
    public bool IsClustered { get; set; } = true;
}