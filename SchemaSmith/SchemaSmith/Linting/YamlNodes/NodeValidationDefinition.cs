using SchemaSmith.Linting.ValidationComponents;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Linting.YamlNodes;

internal abstract class NodeValidationDefinition 
{
    internal abstract YamlNodeType NodeType { get; }

    internal abstract List<ValidationEvent> Validate(YamlNode node);
}