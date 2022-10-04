using SchemaSmith.CypherStatementExtensions;
using SchemaSmith.Domain;
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
            Properties = new[]
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
            Connections = new[]
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
        await Verifier.Verify(cypherStatements);
    }
}