using System;
using System.Linq;
using SchemaSmith.CypherStatementExtensions;
using SchemaSmith.Domain;
using SchemaSmith.Tests.Fixtures;
using VerifyXunit;
using Xunit;

namespace SchemaSmith.Tests.CypherGeneratorTests;

[UsesVerify]
public class NodeExtensionTests
{
    [Fact]
    public async void GenerateCypher_WithProperties_GeneratesProperCypher()
    {
        // Arrange
        var node = new Node
        {
            Label = "Node",
            Properties = Enum.GetValues<NeoDataType>()
                .Select(x => new Property
                {
                    Name = nameof(x),
                    Type = x
                })
        };

        // Act
        var cypherStatement = node.GenerateCypher();

        // Assert
        await Verifier.Verify(cypherStatement, VerifyFixture.VerifySettings);
    }
    
    [Fact]
    public async void GenerateCypher_WithoutProperties_GeneratesProperCypher()
    {
        // Arrange
        var node = new Node
        {
            Label = "Node",
            Properties = Enumerable.Empty<Property>()
        };

        // Act
        var cypherStatement = node.GenerateCypher();

        // Assert
        await Verifier.Verify(cypherStatement, VerifyFixture.VerifySettings);
    }
}