namespace SchemaSmith.SQLServer.Domain;

public class ForeignKey
{
    public string Name { get; set; } = null!;
    public string SourceColumn { get; set; } = null!;
    public string TargetSchema { get; set; } = null!;
    public string TargetTable { get; set; } = null!;
    public string TargetColumn { get; set; } = null!;
}