using System;
using System.Linq;
using SchemaSmith.Neo4j.Core.ScriptGeneration.ExtensionMethods;
using SchemaSmith.Neo4j.Domain.Dto;
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
                }).ToList()
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
        };

        // Act
        var cypherStatement = node.GenerateCypher();

        // Assert
        await Verifier.Verify(cypherStatement, VerifyFixture.VerifySettings);
    }
}