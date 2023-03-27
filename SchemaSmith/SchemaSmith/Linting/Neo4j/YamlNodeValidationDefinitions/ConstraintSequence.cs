using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodes;

internal class ConstraintSequence : SequenceMapNodeValidationDefinition
{
    public ConstraintSequence()
    {
        SequenceItemKey = "name";
        ChildValidationDefinition = new Constraint();
    }
}