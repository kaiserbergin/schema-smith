namespace SchemaSmith.SQLServer.Domain;

public class ScriptOptions
{
    public ScriptIntent ScriptIntent { get; set; } = ScriptIntent.Create;
}

public enum ScriptIntent
{
    Create,
    Update
}