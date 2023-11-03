namespace SchemaSmith.Domain.Dto.Validation;

[Flags]
public enum CaseType
{
    Any = 1,
    PascalCase = 2,
    CamelCase = 4,
    SnakeCase = 8,
    PascalSnakeCase = 16,
    ScreamingSnakeCase = 32,
}