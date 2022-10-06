using System.CommandLine;
using SchemaSmith.CLI.Options;
using SchemaSmith.Linting.Neo4j.Validation;
using SchemaSmith.Linting.ValidationComponents;

namespace SchemaSmith.CLI.Commands;

internal class Lint
{
    internal static readonly Command LintCommand;

    static Lint()
    {
        LintCommand = new Command(
            name: "lint",
            description: "Lint neo4j schema file."
        )
        {
            SchemaSmithFileOptions.NeoSchemaFileInfo
        };

        LintCommand.SetHandler(LintNeoSchema, SchemaSmithFileOptions.NeoSchemaFileInfo);
    }

    internal static void LintNeoSchema(FileInfo file)
    {
        List<ValidationEvent> validationEvents = NeoSpecValidator.ValidateNeo4jSpec(file.FullName);

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
                ? ConsoleColor.Red : ConsoleColor.Yellow;

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

            if (validationEvents.Count > 0)
                Environment.Exit(1);
        }
    }
}