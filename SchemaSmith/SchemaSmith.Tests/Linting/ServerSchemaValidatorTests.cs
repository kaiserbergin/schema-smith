using System.IO;
using System.Reflection;
using FluentAssertions;
using SchemaSmith.Linting;
using VerifyXunit;
using Xunit;

namespace SchemaSmith.Tests.Linting;

[UsesVerify]
public class ServerSchemaValidatorTests
{
    private static readonly string _specPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!}/Schemas";
    
    [Fact]
    public async void Validate_ForValidYaml_ReturnsNoErrors()
    {
        // Arrange
        var schemaPath = $"{_specPath}/good-schema.yml";

        // Act
        var result = ServerSchemaValidator.ValidateNeoSpec(schemaPath);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async void Validate_ForInvalidYaml_ReturnsErrors()
    {
        // Arrange
        var schemaPath = $"{_specPath}/bad-schema.yml";

        // Act
        var result = ServerSchemaValidator.ValidateNeoSpec(schemaPath);

        // Assert
        await Verifier.Verify(result);
    }
}