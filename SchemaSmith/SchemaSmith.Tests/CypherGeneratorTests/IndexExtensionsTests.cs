using SchemaSmith.CypherStatementExtensions;
using SchemaSmith.Domain;
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
                Properties = new[] { "someProp", "someOtherProp" }
            }
        };

        // Act
        var cypherStatement = index.GenerateCypher();

        // Assert
        await Verifier.Verify(cypherStatement);
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
                Properties = new[] { "someProp", "someOtherProp" }
            }
        };

        // Act
        var cypherStatement = index.GenerateCypher();

        // Assert
        await Verifier.Verify(cypherStatement);
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
                Properties = new[] { "textIndex" }
            }
        };

        // Act
        var cypherStatement = index.GenerateCypher();

        // Assert
        await Verifier.Verify(cypherStatement);
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
                Properties = new[] { "point" }
            }
        };

        // Act
        var cypherStatement = index.GenerateCypher();

        // Assert
        await Verifier.Verify(cypherStatement);
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
                Properties = new[] { "rangeProp", "rangeProp2" }
            }
        };

        // Act
        var cypherStatement = index.GenerateCypher();

        // Assert
        await Verifier.Verify(cypherStatement);
    }
}