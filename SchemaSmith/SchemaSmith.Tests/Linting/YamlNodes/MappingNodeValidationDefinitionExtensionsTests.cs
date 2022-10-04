using System.Collections.Generic;
using FluentAssertions;
using SchemaSmith.Linting.YamlNodes;
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
                { "one", new ScalarNodeValidationDefinition() },
                { "two", new ScalarNodeValidationDefinition() },
            },
            AllowedProperties = new Dictionary<string, NodeValidationDefinition>
            {
                { "three", new ScalarNodeValidationDefinition() },
            },
        };
    }
    
    [Fact]
    public void Validate_GoodNode_ReturnsEmptyValidationEvents()
    {
        // Arrange
        var node = new YamlMappingNode();
        
        node.Children.Add(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("one"), new YamlScalarNode()));
        node.Children.Add(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("two"), new YamlScalarNode()));
        node.Children.Add(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("three"), new YamlScalarNode()));
        
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
        
        node.Children.Add(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("one"), new YamlScalarNode()));
        node.Children.Add(new KeyValuePair<YamlNode, YamlNode>(new YamlScalarNode("four"), new YamlScalarNode()));
        
        // Act
        var result = _definition.Validate(node);
        
        // Assert
        await Verifier.Verify(result);
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