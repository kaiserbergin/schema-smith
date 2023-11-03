using SchemaSmith.Domain.Dto.Validation;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Core.Linting.YamlNodes;

public abstract class NodeValidationDefinition 
{
    public abstract YamlNodeType NodeType { get; }

    public abstract List<ValidationEvent> Validate(YamlNode node);
}