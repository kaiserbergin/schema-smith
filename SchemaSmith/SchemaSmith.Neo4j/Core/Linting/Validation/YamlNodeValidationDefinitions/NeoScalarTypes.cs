using SchemaSmith.Core.Linting.YamlNodes;
using SchemaSmith.Domain.Dto.Validation;

namespace SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

public class PropertyKey : ScalarNodeValidationDefinition
{
    public override CaseType CaseType => CaseType.CamelCase | CaseType.SnakeCase;
}

public class LabelName : ScalarNodeValidationDefinition
{
    public override CaseType CaseType => CaseType.PascalCase;
}

public class RelationshipType : ScalarNodeValidationDefinition
{
    public override CaseType CaseType => CaseType.ScreamingSnakeCase;
}

public class ConstraintName : ScalarNodeValidationDefinition
{
    public override CaseType CaseType => CaseType.PascalSnakeCase;
}

public class IndexName : ScalarNodeValidationDefinition
{
    public override CaseType CaseType => CaseType.PascalSnakeCase;
}