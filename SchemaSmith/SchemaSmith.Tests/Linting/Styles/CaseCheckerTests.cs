using FluentAssertions;
using SchemaSmith.Linting.Styles;
using Xunit;

namespace SchemaSmith.Tests.Linting.Styles;

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
    [InlineData("snake__case", CaseType.Any)]
    [InlineData("not_A_CamelCaseOrSnake", CaseType.Any)]
    [InlineData("SCREAMING__SNAKE", CaseType.Any)]
    [InlineData("Pascal__Snake_is_BEST", CaseType.Any)]
    [InlineData("Pascal_Snake__Is_Best", CaseType.Any)]
    [InlineData("kebab-case", CaseType.Any)]
    [InlineData("1dontStartWithNumber", CaseType.Any)]
    [InlineData("no-Special?Characters*I$SaidExcept_", CaseType.Any)]
    [InlineData("def NoSpaces", CaseType.Any)]
    internal void GetCase_WithInvalidCasing_ReturnsInvalidCaseType(string s, CaseType expectedCase)
    {
        CaseChecker.GetCase(s).Should().Be(expectedCase);
    }
}