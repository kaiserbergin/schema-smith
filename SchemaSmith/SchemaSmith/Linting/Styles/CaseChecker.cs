using System.Text.RegularExpressions;

namespace SchemaSmith.Linting.Styles;

internal static class CaseChecker
{
    private static class CasePatterns
    {
        internal static string _pascal = @"^[A-Z][A-Z|a-z|\d]*$";
        internal static string _snakeCase = @"^[a-z](?=[a-z|_|\d]+)(?!.*__)[a-z|_|\d]*$";
        internal static string _camelCase = @"^[a-z][A-Z|a-z|\d]*$";
        internal static string _screamingSnakeCasePattern = @"^[A-Z](?=[A-Z|_|\d]*)(?!.*__)[A-Z|_|\d]*$";
        internal static string _pascalSnake = @"^[A-Z](?=[A-Z|a-z|\d|_]*)(?!.*__)[A-Z|a-z|\d|_]*$";
    }

    internal static CaseType GetCase(string? s)
    {
        var caseType = CaseType.Any;

        if (s is null)
            return caseType;
        
        if (Regex.Match(s, CasePatterns._snakeCase).Success)
            caseType |= CaseType.SnakeCase;
        
        if (Regex.Match(s, CasePatterns._screamingSnakeCasePattern).Success)
            caseType |= CaseType.ScreamingSnakeCase;

        if (Regex.Match(s, CasePatterns._pascal).Success)
            caseType |= CaseType.PascalCase;

        if (Regex.Match(s, CasePatterns._camelCase).Success)
            caseType |= CaseType.CamelCase;

        if (Regex.Match(s, CasePatterns._pascalSnake).Success && !caseType.HasFlag(CaseType.ScreamingSnakeCase))
            caseType |= CaseType.PascalSnakeCase;

        return caseType;
    }
}

[Flags]
internal enum CaseType
{
    Any = 1,
    PascalCase = 2,
    CamelCase = 4,
    SnakeCase = 8,
    PascalSnakeCase = 16,
    ScreamingSnakeCase = 32,
}