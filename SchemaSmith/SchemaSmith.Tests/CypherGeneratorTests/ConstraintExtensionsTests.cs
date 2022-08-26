using System.Threading.Tasks;
using SchemaSmith.CypherGenerator;
using VerifyXunit;
using Xunit;
using SchemaSmith.Domain;

namespace SchemaSmith.Tests.CypherGeneratorTests;

[UsesVerify]
public class ConstraintExtensionsTests
{
    [Fact]
    public async void GenerateCypher_WithNodeKeyConstraint_CreatesProperCypher()
    {
        // Arrange
        var constraint = new Constraint
        {
            Name = nameof(GenerateCypher_WithNodeKeyConstraint_CreatesProperCypher),
            Type = ConstraintType.NodeKey,
            Entity = new Entity
            {
                Type = EntityType.Node,
                Id = "PokemonTrainer",
                Properties = new[] { "trainerId", "internationalId" }
            }
        };
        
        // Act
        var cypherStatement = constraint.GenerateCypher();
        
        // Assert
        await Verifier.Verify(cypherStatement);
    }
    
    [Fact]
    public async void GenerateCypher_WithUniqueKeyConstraint_CreatesProperCypher()
    {
        // Arrange
        var constraint = new Constraint
        {
            Name = nameof(GenerateCypher_WithUniqueKeyConstraint_CreatesProperCypher),
            Type = ConstraintType.Unique,
            Entity = new Entity
            {
                Type = EntityType.Node,
                Id = "PokemonTrainer",
                Properties = new[] { "trainerId", "internationalId" }
            }
        };
        
        // Act
        var cypherStatement = constraint.GenerateCypher();
        
        // Assert
        await Verifier.Verify(cypherStatement);
    }
    
    [Fact]
    public async void GenerateCypher_WithRelationshipPropertyExistenceConstraint_CreatesProperCypher()
    {
        // Arrange
        var constraint = new Constraint
        {
            Name = nameof(GenerateCypher_WithRelationshipPropertyExistenceConstraint_CreatesProperCypher),
            Type = ConstraintType.Existence,
            Entity = new Entity
            {
                Type = EntityType.Relationship,
                Id = "BELONGS_TO_LEAGUE",
                Properties = new[] { "joined" }
            }
        };
        
        // Act
        var cypherStatement = constraint.GenerateCypher();
        
        // Assert
        await Verifier.Verify(cypherStatement);
    }
}