using System.CommandLine;
using NJsonSchema.Validation;
using SchemaSmith.CLI.Options;
using SchemaSmith.IO;
using SchemaSmith.Linting;
using SchemaSmith.Linting.Neo4j;
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
        List<ValidationError> validationErrors = ServerSchemaValidator
            .ValidateNeoSpecStructure(file.FullName)
            .ToList();

        List<ValidationEvent> validationEvents = new List<ValidationEvent>();

        if (validationErrors.Count == 0)
        {
            Program.ServerSchema = SpecReader.GetServerSchemaFromPath(file.FullName);
            validationEvents = NamingConventionValidator
                .ValidateNamingConventions(Program.ServerSchema)
                .ToList();
        }


        List<ValidationEvent> validationEventsErrorLevel = validationEvents
            .Where(e => e.Severity is ValidationSeverity.Error)
            .ToList();

        Console.ResetColor();

        if (validationErrors.Count == 0 && validationEvents.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  \x2714  Schema checks out!");
        }
        else
        {
            if (validationErrors.Count > 0 || validationEventsErrorLevel.Count > 0)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("  \x274C  Schema has issues:");

            validationErrors.ForEach(Console.WriteLine);

            validationEvents
                .Where(e => e.Severity is ValidationSeverity.Error)
                .ToList()
                .ForEach(Console.WriteLine);

            Console.ForegroundColor = ConsoleColor.Yellow;

            validationEvents
                .Where(e => e.Severity is ValidationSeverity.Warning)
                .ToList()
                .ForEach(Console.WriteLine);

            Console.ResetColor();
        }

        Console.ResetColor();

        if (validationErrors.Any() || validationEvents.Any())
            Environment.Exit(1);
    }
}