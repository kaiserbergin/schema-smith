using System.Collections.Generic;
using FluentAssertions;
using SchemaSmith.Core.Linting.YamlNodes;
using SchemaSmith.Domain.Dto.Validation;
using VerifyXunit;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Tests.Linting.YamlNodes;

[UsesVerify]
public class ScalarNodeValidationDefinitionTests
{
    private readonly CaseBasedScalarDefinition _caseBasedScalarDefinitionDefinition;
    private readonly ComboScalarDefinition _comboDefinition;

    public ScalarNodeValidationDefinitionTests()
    {
        _caseBasedScalarDefinitionDefinition = new CaseBasedScalarDefinition();
        _comboDefinition = new ComboScalarDefinition();
    }

    public class CaseBasedScalarDefinition : ScalarNodeValidationDefinition
    {
        public override CaseType CaseType => CaseType.CamelCase | CaseType.SnakeCase;
    }

    public class ComboScalarDefinition : ScalarNodeValidationDefinition
    {
        public override CaseType CaseType => CaseType.CamelCase;

        public ComboScalarDefinition()
        {
            RegexConstraint = "Pickle";
            AllowedEnumeration = new HashSet<string> { "somePickle", "sourPickle", "sweetPickle" };
        }
    }
    
    [Fact]
    public void Validate_GoodNode_ReturnsEmptyValidationEvents()
    {
        // Arrange
        var node = new YamlScalarNode("camelCased");
        
        // Act
        var result = _caseBasedScalarDefinitionDefinition.Validate(node);
        
        // Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async void Validate_ComboCompliant_ReturnsValidationEvents()
    {
        // Arrange
        var node = new YamlScalarNode("sourPickle");
        
        // Act
        var result = _comboDefinition.Validate(node);
        
        // Assert
        await Verifier.Verify(result);
    }
    
    [Fact]
    public async void Validate_BadCaseStyle_ReturnsValidationEvents()
    {
        // Arrange
        var node = new YamlScalarNode("YOU_DONT_WORK_HERE");
        
        // Act
        var result = _caseBasedScalarDefinitionDefinition.Validate(node);
        
        // Assert
        await Verifier.Verify(result);
    }
    
    [Fact]
    public async void Validate_ComboOffender_ReturnsValidationEvents()
    {
        // Arrange
        var node = new YamlScalarNode("DONT_even_MatCh_a-th1ng!");
        
        // Act
        var result = _comboDefinition.Validate(node);
        
        // Assert
        await Verifier.Verify(result);
    }
    
    [Fact]
    public async void Validate_InvalidNodeType_ReturnsValidationEvents()
    {
        // Arrange
        var node = new YamlMappingNode();
        
        // Act
        var result = _caseBasedScalarDefinitionDefinition.Validate(node);
        
        // Assert
        await Verifier.Verify(result);
    }
}