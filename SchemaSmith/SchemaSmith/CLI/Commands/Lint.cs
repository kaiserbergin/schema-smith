using System.CommandLine;
using NJsonSchema.Validation;
using SchemaSmith.CLI.Options;
using SchemaSmith.Linting;

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
             SchemaFileOptions.NeoSchemaFileInfo
         };       
        
        LintCommand.SetHandler(LintNeoSchema, SchemaFileOptions.NeoSchemaFileInfo);
    }

    internal static void LintNeoSchema(FileInfo file)
    {
        List<ValidationError> validationErrors = ServerSchemaValidator
            .ValidateNeoSpec(file.FullName)
            .ToList();

        Console.ResetColor();

        if (validationErrors.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  \x2714  Schema checks out!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  \x274C  Schema has errors:");
            validationErrors.ForEach(Console.WriteLine);
        }

        Console.ResetColor();
        
        if (validationErrors.Any())
            Environment.Exit(1);
    }
}