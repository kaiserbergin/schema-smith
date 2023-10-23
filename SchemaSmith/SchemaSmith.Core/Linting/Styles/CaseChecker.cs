using System.Text.RegularExpressions;
using SchemaSmith.Domain.Dto.Validation;

namespace SchemaSmith.Core.Linting.Styles;

public static class CaseChecker
{
    public static class CasePatterns
    {
        public static string _pascal = @"^[A-Z][A-Z|a-z|\d]*$";
        public static string _snakeCase = @"^[a-z](?=[a-z|_|\d]+)(?!.*__)[a-z|_|\d]*$";
        public static string _camelCase = @"^[a-z][A-Z|a-z|\d]*$";
        public static string _screamingSnakeCasePattern = @"^[A-Z](?=[A-Z|_|\d]*)(?!.*__)[A-Z|_|\d]*$";
        public static string _pascalSnake = @"^[A-Z](?=[A-Z|a-z|\d|_]*)(?!.*__)[A-Z|a-z|\d|_]*$";
    }

    public static CaseType GetCase(string? s)
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

