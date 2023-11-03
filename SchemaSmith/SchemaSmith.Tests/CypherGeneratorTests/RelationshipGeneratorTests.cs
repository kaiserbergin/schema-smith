using System.Collections.Generic;
using SchemaSmith.Neo4j.Core.ScriptGeneration.ExtensionMethods;
using SchemaSmith.Neo4j.Domain.Dto;
using SchemaSmith.Tests.Fixtures;
using VerifyXunit;
using Xunit;

namespace SchemaSmith.Tests.CypherGeneratorTests;

[UsesVerify]
public class RelationshipGeneratorTests
{
    [Fact]
    public async void GenerateCypher_ForRelationshipProperty_GeneratesProperCypher()
    {
        // Arrange
        var relationship = new Relationship
        {
            Type = "TEST_RELATIONSHIP",
            Properties = new List<Property>()
            {
                new Property
                {
                    Name = "propOne",
                    Type = NeoDataType.String
                },
                new Property
                {
                    Name = "propTwo",
                    Type = NeoDataType.Boolean
                }
            },
            Connections = new HashSet<string>()
            {
                "Node->Node",
                "Node<-Node",
                "Node->OtherNode",
                "OtherNode->Node",
                "Node--Directionless"
            }
        };

        // Act
        var cypherStatements = relationship.GenerateCypher();

        // Assert
        await Verifier.Verify(cypherStatements, VerifyFixture.VerifySettings);
    }
}