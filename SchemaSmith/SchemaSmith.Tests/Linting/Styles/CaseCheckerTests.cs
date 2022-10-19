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
    [InlineData("c", CaseType.CamelCase)]
    [InlineData("SCREAMING_SNAKE_CASE", CaseType.ScreamingSnakeCase)]
    [InlineData("SCREAMINGSNAKE", CaseType.ScreamingSnakeCase)]
    [InlineData("S", CaseType.ScreamingSnakeCase)]
    [InlineData("Pascal_Snake_Is_Best", CaseType.PascalSnakeCase)]
    internal void GetCase_WithValidCasing_ReturnsCorrectCaseType(string s, CaseType expectedCase)
    {
        CaseChecker.GetCase(s).HasFlag(expectedCase).Should().BeTrue();
    }
    
    [Theory]
    [InlineData("snake__case")]
    [InlineData("not_A_CamelCaseOrSnake")]
    [InlineData("SCREAMING__SNAKE")]
    [InlineData("Pascal__Snake_is_BEST")]
    [InlineData("Pascal_Snake__Is_Best")]
    [InlineData("kebab-case")]
    [InlineData("1dontStartWithNumber")]
    [InlineData("no-Special?Characters*I$SaidExcept_")]
    [InlineData("def NoSpaces")]
    internal void GetCase_WithInvalidCasing_ReturnsInvalidCaseType(string s)
    {
        CaseChecker.GetCase(s).Should().Be(CaseType.Any);
    }
}