namespace SchemaSmith.Linting.ValidationComponents;

public record ValidationEvent
{
    public ValidationSeverity Severity { get; init; }
    
    public string Message { get; init; } = null!;
    
    public (int Line, int Column) Position { get; init; }

    public override string ToString()
    {
        return $"{Severity}: {Message} at ({Position.Line}, {Position.Column})";
    }
}

public enum ValidationSeverity
{
    Information,
    Warning,
    Error
}