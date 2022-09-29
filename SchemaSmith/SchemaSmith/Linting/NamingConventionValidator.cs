using SchemaSmith.Domain;
using Index = SchemaSmith.Domain.Index;

namespace SchemaSmith.Linting;

internal static class NamingConventionValidator
{
    internal static IEnumerable<ValidationEvent> ValidateNamingConventions(ServerSchema serverSchema)
    {
        var validationEvents = new List<ValidationEvent>();

        foreach (var graph in serverSchema.Graphs)
        {
            validationEvents.AddNodeValidationEvents(graph.Nodes);
            validationEvents.AddRelationshipValidationEvents(graph.Relationships);
            validationEvents.AddConstraintValidationEvents(graph.Constraints);
            validationEvents.AddIndexValidationEvents(graph.Indexes);
        }

        return validationEvents;
    }

    private static void AddNodeValidationEvents(this List<ValidationEvent> events, IEnumerable<Node> nodes)
    {
        foreach (var node in nodes)
        {
            if (CaseChecker.GetCase(node.Label) is not CaseType.PascalCase)
                events.Add(new ValidationEvent
                {
                    Severity = ValidationSeverity.Warning,
                    Message = $"Node labels must be PascalCase. Found {node.Label}"
                });

            events.AddPropertyValidationEvents(node);
        }
    }

    private static void AddPropertyValidationEvents(this List<ValidationEvent> events, Node node)
    {
        foreach (var property in node.Properties)
        {
            if (CaseChecker.GetCase(property.Name) is not CaseType.CamelCase and not CaseType.SnakeCase)
                events.Add(new ValidationEvent
                {
                    Severity = ValidationSeverity.Warning,
                    Message = $"Node properties must be PascalCase or snake_case. Found {property.Name} on {node.Label}"
                });
        }
    }

    private static void AddRelationshipValidationEvents(this List<ValidationEvent> events, IEnumerable<Relationship> relationships)
    {
        foreach (var relationship in relationships)
        {
            if (CaseChecker.GetCase(relationship.Type) is not CaseType.ScreamingSnakeCase)
                events.Add(new ValidationEvent
                {
                    Severity = ValidationSeverity.Warning,
                    Message = $"Relationship types must be SCREAMING_SNAKE_CASE. Found {relationship.Type}"
                });

            events.AddPropertyValidationEvents(relationship);
        }
    }

    private static void AddPropertyValidationEvents(this List<ValidationEvent> events, Relationship relationship)
    {
        foreach (var property in relationship.Properties)
        {
            if (CaseChecker.GetCase(property.Name) is not CaseType.CamelCase and not CaseType.SnakeCase)
                events.Add(new ValidationEvent
                {
                    Severity = ValidationSeverity.Warning,
                    Message = $"Relationship properties must be camelCase or snake_case. Found {property.Name} on {relationship.Type}"
                });
        }
    }

    private static void AddConstraintValidationEvents(this List<ValidationEvent> events, IEnumerable<Constraint> constraints)
    {
        foreach (var constraint in constraints)
        {
            if (CaseChecker.GetCase(constraint.Name) is not CaseType.PascalSnakeCase)
                events.Add(new ValidationEvent
                {
                    Severity = ValidationSeverity.Warning,
                    Message = $"Constraint names must be Pascal_Snake_Case. Found {constraint.Name}"
                });
        }
    }

    private static void AddIndexValidationEvents(this List<ValidationEvent> events, IEnumerable<Index> indexes)
    {
        foreach (var index in indexes)
        {
            if (CaseChecker.GetCase(index.Name) is not CaseType.PascalSnakeCase)
                events.Add(new ValidationEvent
                {
                    Severity = ValidationSeverity.Warning,
                    Message = $"Index names must be Pascal_Snake_Case. Found {index.Name}"
                });
        }
    }
}