using System;
using System.Linq;
using SchemaSmith.CypherGenerator;
using SchemaSmith.Domain;
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
        var properties = Enum.GetValues<PropertyType>()
            .Select(x => new Property
            {
                Name = nameof(x),
                Type = x
            });
        
        // Act
        var defaultValues = properties.Select(x => x.GenerateDefaultPropertyValue()).ToList();

        // Assert
        await Verifier.Verify(defaultValues);
    }
}