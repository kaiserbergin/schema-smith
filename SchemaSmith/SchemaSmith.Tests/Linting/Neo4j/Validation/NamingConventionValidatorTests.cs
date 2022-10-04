using System.IO;
using System.Reflection;
using FluentAssertions;
using SchemaSmith.IO;
using SchemaSmith.Linting;
using SchemaSmith.Linting.Neo4j.Validation;
using VerifyXunit;
using Xunit;

namespace SchemaSmith.Tests.Linting.Neo4j.Validation;

[UsesVerify]
public class NamingConventionValidatorTests
{
    private static readonly string _specPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!}/Schemas";
    
    [Fact]
    public async void Validate_ForValidYaml_ReturnsNoErrors()
    {
        // Arrange
        var schemaPath = $"{_specPath}/good-schema.yml";
        var serverSchema = SpecReader.GetServerSchemaFromPath(schemaPath);

        // Act
        var result = NamingConventionValidator.ValidateNamingConventions(serverSchema);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async void Validate_ForInvalidYaml_ReturnsErrors()
    {
        // Arrange
        var schemaPath = $"{_specPath}/bad-naming-conventions.yml";
        var serverSchema = SpecReader.GetServerSchemaFromPath(schemaPath);

        // Act
        var result = NamingConventionValidator.ValidateNamingConventions(serverSchema);

        // Assert
        await Verifier.Verify(result);
    }
}