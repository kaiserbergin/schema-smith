using System.IO;
using System.Reflection;
using SchemaSmith.Infrastructure.IO;
using VerifyXunit;
using Xunit;
using SchemaSmith.Neo4j.Domain.Dto;

namespace SchemaSmith.Tests.IO;

[UsesVerify]
public class SpecReaderTests
{
    [Fact]
    public async void GetServerSchemaFromPath_WithValidSchema_ReturnsDeserializedSchema()
    {
        // Arrange
        var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var specPath = $@"{buildDir}/Schemas/good-schema.yml";

        // Act
        var serverSchema = SpecReader.GetServerSchemaFromPath<ServerSchema>(specPath);

        // Assert
        await Verifier.Verify(serverSchema);
    }

    [Fact]
    public async void GetServerSchemaText_WithValidSchema_ReturnsDeserializedSchema()
    {
        // Arrange
        var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var specPath = $@"{buildDir}/Schemas/good-schema.yml";

        // Act
        var serverSchema = SpecReader.GetServerSchemaText(specPath);

        // Assert
        await Verifier.Verify(serverSchema);
    }
    
    [Fact]
    public async void GetServerSchemaAsJsonFromPath_WithValidSchema_ReturnsDeserializedSchema()
    {
        // Arrange
        var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var specPath = $@"{buildDir}/Schemas/good-schema.yml";

        // Act
        var serverSchema = SpecReader.GetServerSchemaAsJsonFromPath(specPath);

        // Assert
        await Verifier.VerifyJson(serverSchema);
    }

    [Fact]
    public async void TestyTest()
    {
        // Arrange
        var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var specPath = $@"{buildDir}/Schemas/good-schema.yml";

        // Act
        SpecReader.GetYamlMapping(specPath);
    }
}