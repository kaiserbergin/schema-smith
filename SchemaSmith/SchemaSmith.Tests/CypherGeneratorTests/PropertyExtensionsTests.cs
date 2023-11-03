using System;
using System.Linq;
using SchemaSmith.Neo4j.Core.ScriptGeneration.ExtensionMethods;
using SchemaSmith.Neo4j.Domain.Dto;
using SchemaSmith.Tests.Fixtures;
using VerifyXunit;
using Xunit;

namespace SchemaSmith.Tests.CypherGeneratorTests;

[UsesVerify]
public class PropertyExtensionsTests
{
    [Fact]
    public async void GenerateDefaultPropertyValue_WithAllTypes_GeneratesDefaultValues()
    {
        // Arrange
        var properties = Enum.GetValues<NeoDataType>()
            .Select(x => new Property
            {
                Name = nameof(x),
                Type = x
            });
        
        // Act
        var defaultValues = properties.Select(x => x.GenerateDefaultPropertyValue()).ToList();

        // Assert
        await Verifier.Verify(defaultValues, VerifyFixture.VerifySettings);
    }
}