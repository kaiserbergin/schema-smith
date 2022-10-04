using FluentAssertions;
using SchemaSmith.Linting.Styles;
using SchemaSmith.Linting.YamlNodes;
using VerifyXunit;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Tests.Linting.YamlNodes;

[UsesVerify]
public class SequenceScalarNodeValidationDefinitionTests
{
    private readonly SequenceScalarNodeValidationDefinition _definition;

    public SequenceScalarNodeValidationDefinitionTests()
    {
        _definition = new SequenceScalarNodeValidationDefinition
        {
            ChildValidationDefinition = new ScalarNodeValidationDefinition
            {
                CaseType = CaseType.CamelCase
            }
        };
    }

    [Fact]
    public void Validate_WithValidChildNodes_ReturnsEmptyValidationEvents()
    {
        // Arrange
        var childNode1 = new YamlScalarNode("camelCase");
        var childNode2 = new YamlScalarNode("alsoCamelCased");

        var sequenceNode = new YamlSequenceNode(childNode1, childNode2);

        // Act
        var validationEvents = _definition.Validate(sequenceNode);

        // Assert
        validationEvents.Should().BeEmpty();
    }
    
    [Fact]
    public async void Validate_WithInvalidChildren_ReturnsEmptyValidationEvents()
    {
        // Arrange
        var childNode1 = new YamlScalarNode("camelCase");
        var childNode2 = new YamlScalarNode("SREAMING_CAMEL_NOT");
        var childNode3 = new YamlMappingNode();

        var sequenceNode = new YamlSequenceNode(childNode1, childNode2, childNode3);

        // Act
        var validationEvents = _definition.Validate(sequenceNode);

        // Assert
        await Verifier.Verify(validationEvents);
    }
}