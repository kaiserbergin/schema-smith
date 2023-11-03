using System;
using FluentAssertions;
using SchemaSmith.Neo4j.CLI.Options;
using Xunit;

namespace SchemaSmith.Tests.CLI.Options;

public class Neo4jConnectionOptionsTests
{
    [Theory]
    [InlineData("50", 50)]
    [InlineData("50ms", 50)]
    [InlineData("50s", 50 * 1_000)]
    [InlineData("10m", 10 * 1_000 * 60)]
    public void GetMillisecondsFromTimeoutArg_WithValidTimeout_ReturnsCorrectMilliseconds(string timeout,
        int expectedMilliseconds) =>
        Neo4jConnectionOptions.GetMillisecondsFromTimeoutArg(timeout)
            .Should()
            .Be(expectedMilliseconds);

    [Theory]
    [InlineData("50x")]
    [InlineData("50xms")]
    [InlineData("50sx")]
    [InlineData("5.0s")]
    [InlineData("50sms")]
    [InlineData("50sm")]
    public void GetMillisecondsFromTimeoutArg_WithInvalidTimeout_ThrowsFormatException(string timeout)
    {
        var call = () => Neo4jConnectionOptions.GetMillisecondsFromTimeoutArg(timeout);

        call.Should().Throw<Exception>()
            .WithMessage(Neo4jConnectionOptions.TimeoutStringFormattingExceptionMessage);
    }
}