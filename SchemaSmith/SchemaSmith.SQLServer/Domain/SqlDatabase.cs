namespace SchemaSmith.SQLServer.Domain;

public class SqlDatabase
{
    public string DatabaseName { get; set; } = null!;
    public string? Description { get; set; }
    public List<Schema> Schemas { get; set; } = new();
}