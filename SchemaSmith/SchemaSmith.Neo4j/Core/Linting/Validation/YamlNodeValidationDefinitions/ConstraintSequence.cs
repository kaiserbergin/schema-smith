using SchemaSmith.Core.Linting.YamlNodes;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class ConstraintSequence : SequenceMapNodeValidationDefinition
{
    public ConstraintSequence()
    {
        SequenceItemKey = "name";
        ChildValidationDefinition = new Constraint();
    }
}