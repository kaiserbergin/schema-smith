using System.Collections.Generic;
using FluentAssertions;
using SchemaSmith.Core.Linting.YamlNodes;
using SchemaSmith.Domain.Dto.Validation;
using VerifyXunit;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Tests.Linting.YamlNodes;

[UsesVerify]
public class MappingNodeValidationDefinitionExtensionsTests
{
    private readonly MappingNodeValidationDefinition _definition;

    public MappingNodeValidationDefinitionExtensionsTests()
    {
        _definition = new MappingNodeValidationDefinition
        {
            RequiredProperties = new Dictionary<string, NodeValidationDefinition>
            {
                { "one", new ScalarNodeValidationDefinition { AllowedEnumeration = new HashSet<string> { "value", "prop", "roi" } } },
                { "two", new ScalarNodeValidationDefinition { CaseType = CaseType.CamelCase } },
            },
            AllowedProperties = new Dictionary<string, NodeValidationDefinition>
            {
                {
                    "three", 
                    new SequenceScalarNodeValidationDefinition
                    {
                        ChildValidationDefinition = new ScalarNodeValidationDefinition
                        {
                            CaseType = CaseType.ScreamingSnakeCase
                        }
                    }
                },
                {
                    "four", 
                    new SequenceMapNodeValidationDefinition
                    {
                        ChildValidationDefinition = new MappingNodeValidationDefinition
                        {
                            RequiredProperties = new Dictionary<string, NodeValidationDefinition>
                            {
                                { "id", new ScalarNodeValidationDefinition() }
                            },
                            AllowedProperties = new Dictionary<string, NodeValidationDefinition>
                            {
                                { "value", new ScalarNodeValidationDefinition() }
                            }
                        },
                        SequenceItemKey = "id"
                    }
                },
            },
        };
    }

    [Fact]
    public void Validate_GoodNode_ReturnsEmptyValidationEvents()
    {
        // Arrange
        var node = new YamlMappingNode();

        node.Children.Add(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("one"), new YamlScalarNode("value")));
        node.Children.Add(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("two"), new YamlScalarNode("camelCase")));
        node.Children.Add(new KeyValuePair<YamlNode, YamlNode>(
                new YamlScalarNode("three"),
                new YamlSequenceNode(new YamlScalarNode("SCREAMING_SNAKE"))
            )
        );
        node.Children.Add(new KeyValuePair<YamlNode, YamlNode>(
                new YamlScalarNode("four"),
                new YamlSequenceNode(
                    new YamlMappingNode(
                        new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("id"), new YamlScalarNode("one")),
                        new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("value"), new YamlScalarNode())
                    ),
                    new YamlMappingNode(
                        new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("id"), new YamlScalarNode("two")),
                        new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("value"), new YamlScalarNode())
                    )
                )
            )
        );

        // Act
        var result = _definition.Validate(node);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async void Validate_InvalidRequiredProperties_ReturnsValidationEvents()
    {
        // Arrange
        var node = new YamlMappingNode();

        node.Children.Add(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("one"), new YamlScalarNode("notMatched")));
        node.Children.Add(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("two"), new YamlScalarNode("BAD_CASE")));
        node.Children.Add(new KeyValuePair<YamlNode, YamlNode>(
                new YamlScalarNode("three"),
                new YamlScalarNode("Bad node type.")
            )
        );
        node.Children.Add(new KeyValuePair<YamlNode, YamlNode>(
                new YamlScalarNode("four"),
                new YamlSequenceNode(
                    new YamlMappingNode(
                        new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("id"), new YamlScalarNode("duplicate")),
                        new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("value"), new YamlScalarNode()),
                        new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("not allowed"), new YamlScalarNode())
                    ),
                    new YamlMappingNode(
                        new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("id"), new YamlScalarNode("duplicate")),
                        new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("value"), new YamlScalarNode())
                    ),
                    new YamlMappingNode(
                        new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("requiredNotPresent"), new YamlScalarNode("duplicate"))
                    )
                )
            )
        );
        node.Children.Add(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("not allowed"), new YamlScalarNode("def not")));

        // Act
        var validationEvents = _definition.Validate(node);

        // Assert
        await Verifier.Verify(validationEvents);
    }

    [Fact]
    public async void Validate_InvalidNodeType_ReturnsValidationEvents()
    {
        // Arrange
        var node = new YamlScalarNode();

        // Act
        var result = _definition.Validate(node);

        // Assert
        await Verifier.Verify(result);
    }
}