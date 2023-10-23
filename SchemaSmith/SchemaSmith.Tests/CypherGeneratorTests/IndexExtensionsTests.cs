using System.Collections.Generic;
using SchemaSmith.CypherStatementExtensions;
using SchemaSmith.Domain;
using SchemaSmith.Neo4j.Domain.Dto;
using SchemaSmith.Tests.Fixtures;
using VerifyXunit;
using Xunit;

namespace SchemaSmith.Tests.CypherGeneratorTests;

[UsesVerify]
public class IndexExtensionsTests
{
    [Fact]
    public async void GenerateCypher_IndexForNode_CreatesProperCypher()
    {
        // Arrange
        var index = new Index
        {
            Name = nameof(GenerateCypher_IndexForNode_CreatesProperCypher),
            Type = IndexType.BTree,
            Entity = new Entity
            {
                Type = EntityType.Node,
                Id = "Node",
                Properties = new List<string>() { "someProp", "someOtherProp" }
            }
        };

        // Act
        var cypherStatement = index.GenerateCypher();

        // Assert
        await Verifier.Verify(cypherStatement, VerifyFixture.VerifySettings);
    }
    
    [Fact]
    public async void GenerateCypher_IndexForRelationship_CreatesProperCypher()
    {
        // Arrange
        var index = new Index
        {
            Name = nameof(GenerateCypher_IndexForRelationship_CreatesProperCypher),
            Type = IndexType.BTree,
            Entity = new Entity
            {
                Type = EntityType.Relationship,
                Id = "Node",
                Properties = new List<string>() { "someProp", "someOtherProp" }
            }
        };

        // Act
        var cypherStatement = index.GenerateCypher();

        // Assert
        await Verifier.Verify(cypherStatement, VerifyFixture.VerifySettings);
    }
    
    [Fact]
    public async void GenerateCypher_TextIndexForNode_CreatesProperCypher()
    {
        // Arrange
        var index = new Index
        {
            Name = nameof(GenerateCypher_TextIndexForNode_CreatesProperCypher),
            Type = IndexType.Text,
            Entity = new Entity
            {
                Type = EntityType.Node,
                Id = "Node",
                Properties = new List<string>() { "textIndex" }
            }
        };

        // Act
        var cypherStatement = index.GenerateCypher();

        // Assert
        await Verifier.Verify(cypherStatement, VerifyFixture.VerifySettings);
    }
    
    [Fact]
    public async void GenerateCypher_PointIndexForNode_CreatesProperCypher()
    {
        // Arrange
        var index = new Index
        {
            Name = nameof(GenerateCypher_IndexForNode_CreatesProperCypher),
            Type = IndexType.Point,
            Entity = new Entity
            {
                Type = EntityType.Node,
                Id = "Node",
                Properties = new List<string>() { "point" }
            }
        };

        // Act
        var cypherStatement = index.GenerateCypher();

        // Assert
        await Verifier.Verify(cypherStatement, VerifyFixture.VerifySettings);
    }
    
    [Fact]
    public async void GenerateCypher_RangeIndexForNode_CreatesProperCypher()
    {
        // Arrange
        var index = new Index
        {
            Name = nameof(GenerateCypher_RangeIndexForNode_CreatesProperCypher),
            Type = IndexType.Range,
            Entity = new Entity
            {
                Type = EntityType.Node,
                Id = "Node",
                Properties = new List<string>() { "rangeProp", "rangeProp2" }
            }
        };

        // Act
        var cypherStatement = index.GenerateCypher();

        // Assert
        await Verifier.Verify(cypherStatement, VerifyFixture.VerifySettings);
    }
}