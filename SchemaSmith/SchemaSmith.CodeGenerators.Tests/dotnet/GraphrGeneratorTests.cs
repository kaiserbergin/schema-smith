using CodeGenerators.dotnet;
using FluentAssertions;
using Xunit;

namespace SchemaSmith.CodeGenerators.Tests.dotnet;

public class GraphrGeneratorTests
{
    [Fact]
    public void Test()
    {
        var expected = "pumpkin";
        var generator = new GraphrGenerator(expected);
        var content = generator.TransformText();

        content.Should().Be(expected);
    }
}