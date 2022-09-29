namespace SchemaSmith.Linting;

public record ValidationEvent
{
    public ValidationSeverity Severity { get; init; }
    public string Message { get; init; } = null!;

    public override string ToString()
    {
        return $"{Severity}: {Message}";
    }
}

public enum ValidationSeverity
{
    Information,
    Warning,
    Error
}