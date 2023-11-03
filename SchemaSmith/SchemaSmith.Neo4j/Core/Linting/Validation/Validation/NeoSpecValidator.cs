using SchemaSmith.Domain.Dto.Validation;
using SchemaSmith.Domain.Interfaces;
using SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.Validation;

public class NeoSpecValidator : ISchemaValidator
{
    public List<ValidationEvent> Validate(YamlNode node)
    {
        var validator = new ServerSchemaMap();

        return validator.Validate(node);
    }
}