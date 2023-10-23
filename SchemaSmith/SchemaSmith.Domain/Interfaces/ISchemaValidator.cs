using SchemaSmith.Domain.Dto.Validation;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Domain.Interfaces;

public interface ISchemaValidator
{
    public List<ValidationEvent> Validate(YamlNode schema);
}