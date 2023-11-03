using SchemaSmith.Domain.Dto.Validation;
using SchemaSmith.Domain.Interfaces;
using YamlDotNet.RepresentationModel;

namespace SchemaSmith.Core.Linting;

public class Linter
{
    public static async Task LintAsync(YamlMappingNode rootNode, ISchemaValidator validator)
    {
        List<ValidationEvent> validationEvents = validator.Validate(rootNode);

        var validationEventsErrorLevel = validationEvents
            .Where(e => e.Severity is ValidationSeverity.Error)
            .ToList();

        Console.ResetColor();

        if (validationEvents.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Schema checks out!");
        }
        else
        {
            Console.ForegroundColor = validationEventsErrorLevel.Any(e => e.Severity is ValidationSeverity.Error)
                ? ConsoleColor.Red
                : ConsoleColor.Yellow;

            Console.WriteLine("Schema has issues:\n");

            validationEvents
                .ToList()
                .ForEach(e =>
                {
                    Console.ForegroundColor = e.Severity switch
                    {
                        ValidationSeverity.Error => ConsoleColor.Red,
                        _ => ConsoleColor.Yellow,
                    };

                    Console.WriteLine(e.ToString());
                });

            if (validationEvents.Any(e => e.Severity == ValidationSeverity.Error))
                Environment.Exit(1);
            else
                Console.WriteLine("You should really consider fixing those warnings, but I'll give you a pass.");
        }
    }
}