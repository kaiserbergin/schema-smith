using System.Collections.Generic;
using SchemaSmith.Domain.Interfaces;
using SchemaSmith.Neo4j.Core.ScriptGeneration;
using SchemaSmith.Neo4j.Domain.Dto;
using SchemaSmith.Tests.Fixtures;
using VerifyXunit;
using Xunit;

namespace SchemaSmith.Tests.CypherGeneratorTests;

[UsesVerify]
public class GraphExtensionsTests
{
    private readonly ICreateScriptGenerator<GraphSchema> _createScriptGenerator;

    public GraphExtensionsTests()
    {
        _createScriptGenerator = new CreateScriptGenerator();
    }

    [Fact]
    public async void GenerateCypherStatements_ForGraph_CreatesProperCypher()
    {
        // Arrange
        const string labelOne = "one";
        const string labelTwo = "two";

        var graph = new GraphSchema
        {
            Name = "neo4j",
            Nodes = new List<Node>()
            {
                new Node
                {
                    Label = labelOne,
                    Properties = new List<Property>()
                    {
                        new Property
                        {
                            Name = "one",
                            Type = NeoDataType.String
                        },
                        new Property
                        {
                            Name = "two",
                            Type = NeoDataType.String
                        },
                        new Property
                        {
                            Name = "three",
                            Type = NeoDataType.String
                        }
                    }
                },
                new Node
                {
                    Label = labelTwo,
                    Properties = new List<Property>() { new Property { Name = "indexed", Type = NeoDataType.String } }
                }
            },
            Relationships = new List<Relationship>()
            {
                new Relationship
                {
                    Type = "REL_ONE",
                    Connections = new HashSet<string>()
                        { $"{labelOne}->{labelTwo}", $"{labelTwo}->{labelOne}", $"{labelOne}--{labelOne}" }
                },
                new Relationship
                {
                    Type = "REL_TWO",
                    Connections = new HashSet<string>()
                        { $"{labelOne}->{labelTwo}", $"{labelTwo}->{labelOne}", $"{labelOne}--{labelOne}" }
                }
            },
            Constraints = new List<Constraint>()
            {
                new Constraint
                {
                    Name = "Constraint1",
                    Type = ConstraintType.NodeKey,
                    Entity = new Entity
                    {
                        Type = EntityType.Node,
                        Id = labelOne,
                        Properties = new List<string>() { "one", "two" }
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
                        Properties = new List<string>() { "three" }
                    }
                }
            },
            Indexes = new List<Index>()
            {
                new Index
                {
                    Name = "Index1",
                    Type = IndexType.BTree,
                    Entity = new Entity
                    {
                        Type = EntityType.Node,
                        Id = labelTwo,
                        Properties = new List<string>() { "indexed" }
                    }
                }
            }
        };

        // Act
        var cypherStatements = _createScriptGenerator.GenerateCreateScript(graph);

        // Assert
        await Verifier.Verify(cypherStatements, VerifyFixture.VerifySettings);
    }
}