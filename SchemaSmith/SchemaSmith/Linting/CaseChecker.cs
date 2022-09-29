using System.Text.RegularExpressions;

namespace SchemaSmith.Linting;

internal static class CaseChecker
{
    private static class CasePatterns
    {
        internal static string _pascal = @"^[A-Z][A-Z|a-z|/d]*$";
        internal static string _snakeCase = @"^[a-z](?=[a-z|_|/d]+)(?!.*__)[a-z|_|/d]*$";
        internal static string _camelCase = @"^[a-z][A-Z|a-z|/d]*$";
        internal static string _screamingSnakeCasePattern = @"^[A-Z](?=[A-Z|_|/d]*)(?!.*__)[A-Z|_|/d]*$";
        internal static string _pascalSnake = @"^[A-Z](?=[A-Z|a-z|/d|_]*)(?!.*__)[A-Z|a-z|/d|_]*$";
    }

    internal static CaseType GetCase(string s)
    {
        if (Regex.Match(s, CasePatterns._pascal).Success)
            return CaseType.PascalCase;
        
        if (Regex.Match(s, CasePatterns._snakeCase).Success)
            return CaseType.SnakeCase;

        if (Regex.Match(s, CasePatterns._camelCase).Success)
            return CaseType.CamelCase;
       
        if (Regex.Match(s, CasePatterns._screamingSnakeCasePattern).Success)
            return CaseType.ScreamingSnakeCase;
        
        if (Regex.Match(s, CasePatterns._pascalSnake).Success)
            return CaseType.PascalSnakeCase;
        
        return CaseType.Invalid;
    }
}


internal enum CaseType
{
    Invalid,
    PascalCase,
    CamelCase,
    SnakeCase,
    PascalSnakeCase,
    ScreamingSnakeCase,
}