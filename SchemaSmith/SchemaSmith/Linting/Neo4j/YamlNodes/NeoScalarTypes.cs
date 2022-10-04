using SchemaSmith.Linting.Styles;
using SchemaSmith.Linting.YamlNodes;

namespace SchemaSmith.Linting.Neo4j.YamlNodes;

internal class PropertyKey : ScalarNodeValidationDefinition
{
    internal override CaseType CaseType => CaseType.CamelCase | CaseType.SnakeCase;
}

internal class LabelName : ScalarNodeValidationDefinition
{
    internal override CaseType CaseType => CaseType.PascalCase;
}

internal class RelationshipType : ScalarNodeValidationDefinition
{
    internal override CaseType CaseType => CaseType.ScreamingSnakeCase;
}

internal class ConstraintName : ScalarNodeValidationDefinition
{
    internal override CaseType CaseType => CaseType.PascalSnakeCase;
}

internal class IndexName : ScalarNodeValidationDefinition
{
    internal override CaseType CaseType => CaseType.PascalSnakeCase;
}