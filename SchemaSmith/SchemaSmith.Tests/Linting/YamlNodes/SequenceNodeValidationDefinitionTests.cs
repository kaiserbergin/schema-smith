using System.Collections.Generic;
using FluentAssertions;
using SchemaSmith.Linting.Styles;
using SchemaSmith.Linting.YamlNodes;
using VerifyXunit;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Tests.Linting.YamlNodes;

[UsesVerify]
public class SequenceNodeValidationDefinitionTests
{
    private const string SEQUENCE_KEY = "SEQUENCE_KEY";
    private readonly SequenceMapNodeValidationDefinition _definition;


    public SequenceNodeValidationDefinitionTests()
    {
        _definition = new SequenceMapNodeValidationDefinition
        {
            SequenceItemKey = SEQUENCE_KEY,
        };
    }

    [Fact]
    public void Validate_WithValidSequence_ReturnsEmptyValidationEvents()
    {
        // Act
        var sequenceItem1 = new YamlMappingNode(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode(SEQUENCE_KEY), new YamlScalarNode("one")));
        var sequenceItem2 = new YamlMappingNode(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode(SEQUENCE_KEY), new YamlScalarNode("two")));
        var sequenceItem3 = new YamlMappingNode(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode(SEQUENCE_KEY), new YamlScalarNode("three")));

        var sequenceNode = new YamlSequenceNode(sequenceItem1, sequenceItem2, sequenceItem3);
        
        // Act
        var validationEvents = _definition.Validate(sequenceNode);
        
        // Assert
        validationEvents.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithEmptySequence_ReturnsEmptyValidationEvents()
    {
        // Act
        var sequenceNode = new YamlSequenceNode();
        
        // Act
        var validationEvents = _definition.Validate(sequenceNode);
        
        // Assert
        validationEvents.Should().BeEmpty();
    }
    
    [Fact]
    public async void Validate_WithIssues_ReturnsValidationEvents()
    {
        // Act
        var item1 = new YamlMappingNode(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode(SEQUENCE_KEY), new YamlScalarNode("one")));
        var item2 = new YamlMappingNode(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode(SEQUENCE_KEY), new YamlScalarNode("one")));
        var item3 = new YamlMappingNode(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode(SEQUENCE_KEY), new YamlScalarNode("two")));
        var item4 = new YamlMappingNode();
        var item5 = new YamlScalarNode();

        var sequenceNode = new YamlSequenceNode(item1, item2, item3, item4, item5);
        
        // Act
        var validationEvents = _definition.Validate(sequenceNode);
        
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