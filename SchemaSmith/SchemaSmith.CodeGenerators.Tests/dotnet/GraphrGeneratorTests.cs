using System.Collections.Generic;
using System.Threading.Tasks;
using CodeGenerators.dotnet;
using FluentAssertions;
using SchemaSmith.Domain;
using VerifyXunit;
using Xunit;

namespace SchemaSmith.CodeGenerators.Tests.dotnet;

[UsesVerify]
public class GraphrGeneratorTests
{
    [Fact]
    public async void Test()
    {
        var expected = new ServerSchema
        {
            ServerUrl = "www.test-my-url.com",
            Graphs = new List<GraphSchema>
            {
                new GraphSchema
                {
                    Name = "test",
                    Relationships = new List<Relationship>
                    {
                        new Relationship
                        {
                            Type = "TEST_TYPE",
                            Properties = new List<Property>
                            {
                                new Property
                                {
                                    Name = "PropertyOne",
                                    Type = NeoDataType.String 
                                },
                                new Property
                                {
                                    Name = "PropertyTwo",
                                    Type = NeoDataType.ListString
                                }
                            },
                            Connections = new List<string>
                            {
                                "One->Two",
                                "Three<-Two",
                                "Three-Four"
                            }
                        }
                    }
                }
            }
        };
        var generator = new GraphrGenerator(expected);
        var content = generator.TransformText();

        content.Should().NotBeNullOrEmpty();
        await Verifier.Verify(content);
    }
}