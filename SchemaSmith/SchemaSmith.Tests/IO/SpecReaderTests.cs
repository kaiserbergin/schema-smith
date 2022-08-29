using System.IO;
using System.Reflection;
using SchemaSmith.Domain;
using VerifyXunit;
using Xunit;
using SchemaSmith.IO;
using VerifyTests;

namespace SchemaSmith.Tests.IO;

[UsesVerify]
public class SpecReaderTests
{
    /// <summary>
    /// For some reason, verify ignores the 
    /// </summary>
    [Fact]
    public async void GetSchemaFromPath_WithValidSchema_ReturnsDeserializedSchema()
    {
        // Arrange
        var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var specPath = $@"{buildDir}/IO/good-schema.yml";

        // Act
        var serverSchema = SpecReader.GetSchemaFromPath(specPath);

        // Assert
        await Verifier.Verify(serverSchema);
    }
}