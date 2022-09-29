using FluentAssertions;
using SchemaSmith.Linting;
using Xunit;

namespace SchemaSmith.Tests.Linting;

public class CaseCheckerTests
{
    [Theory]
    [InlineData("snake_case_with_more_things", CaseType.SnakeCase)]
    [InlineData("PascalCase", CaseType.PascalCase)]
    [InlineData("camelCase", CaseType.CamelCase)]
    [InlineData("SCREAMING_SNAKE_CASE", CaseType.ScreamingSnakeCase)]
    [InlineData("Pascal_Snake_Is_Best", CaseType.PascalSnakeCase)]
    internal void GetCase_WithValidCasing_ReturnsCorrectCaseType(string s, CaseType expectedCase)
    {
        CaseChecker.GetCase(s).Should().Be(expectedCase);
    }
    
    [Theory]
    [InlineData("snake__case", CaseType.Invalid)]
    [InlineData("not_A_CamelCaseOrSnake", CaseType.Invalid)]
    [InlineData("SCREAMING__SNAKE", CaseType.Invalid)]
    [InlineData("Pascal__Snake_is_BEST", CaseType.Invalid)]
    [InlineData("Pascal_Snake__Is_Best", CaseType.Invalid)]
    [InlineData("kebab-case", CaseType.Invalid)]
    [InlineData("1dontStartWithNumber", CaseType.Invalid)]
    [InlineData("no-Special?Characters*I$SaidExcept_", CaseType.Invalid)]
    [InlineData("def NoSpaces", CaseType.Invalid)]
    internal void GetCase_WithInvalidCasing_ReturnsInvalidCaseType(string s, CaseType expectedCase)
    {
        CaseChecker.GetCase(s).Should().Be(expectedCase);
    }
}