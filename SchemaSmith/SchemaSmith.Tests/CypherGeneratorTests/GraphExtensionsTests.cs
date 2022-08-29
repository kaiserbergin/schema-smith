using System.Linq;
using SchemaSmith.CypherStatementExtensions;
using SchemaSmith.Domain;
using VerifyXunit;
using Xunit;

namespace SchemaSmith.Tests.CypherGeneratorTests;

[UsesVerify]
public class GraphExtensionsTests
{
    [Fact]
    public async void GenerateCypherStatements_ForGraph_CreatesProperCypher()
    {
        // Arrange
        const string labelOne = "one";
        const string labelTwo = "two";

        var graph = new GraphSchema
        {
            Name = "neo4j",
            Nodes = new[]
            {
                new Node
                {
                    Label = labelOne,
                    Properties = new[]
                    {
                        new Property
                        {
                            Name = "one",
                            Type = PropertyType.String
                        },
                        new Property
                        {
                            Name = "two",
                            Type = PropertyType.String
                        },
                        new Property
                        {
                            Name = "three",
                            Type = PropertyType.String
                        }
                    }
                },
                new Node
                {
                    Label = labelTwo,
                    Properties = new[] { new Property { Name = "indexed", Type = PropertyType.String } }
                }
            },
            Relationships = new[]
            {
                new Relationship
                {
                    Type = "REL_ONE",
                    Properties = Enumerable.Empty<Property>(),
                    Connections = new[] { $"{labelOne}->{labelTwo}", $"{labelTwo}->{labelOne}", $"{labelOne}--{labelOne}" }
                },
                new Relationship
                {
                    Type = "REL_TWO",
                    Properties = Enumerable.Empty<Property>(),
                    Connections = new[] { $"{labelOne}->{labelTwo}", $"{labelTwo}->{labelOne}", $"{labelOne}--{labelOne}" }
                }
            },
            Constraints = new[]
            {
                new Constraint
                {
                    Name = "Constraint1",
                    Type = ConstraintType.NodeKey,
                    Entity = new Entity
                    {
                        Type = EntityType.Node,
                        Id = labelOne,
                        Properties = new[] { "one", "two" }
                    }
                },
                new Constraint
                {
                    Name = "Constraint2",
                    Type = ConstraintType.Unique,
                    Entity = new Entity
                    {
                        Type = EntityType.Node,
                        Id = labelOne,
                        Properties = new[] { "three" }
                    }
                }
            },
            Indexes = new[]
            {
                new Index
                {
                    Name = "Index1",
                    Type = IndexType.BTree,
                    Entity = new Entity
                    {
                        Type = EntityType.Node,
                        Id = labelTwo,
                        Properties = new[] { "indexed" }
                    }
                }
            }
        };

        // Act
        var cypherStatements = graph.GenerateCypherStatements();

        // Assert
        await Verifier.Verify(cypherStatements);
    }
}