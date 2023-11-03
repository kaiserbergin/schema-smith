using System.CommandLine;
using SchemaSmith.Core.Linting;
using SchemaSmith.Domain.Dto.Validation;
using SchemaSmith.Neo4j.CLI.Options;
using SchemaSmith.Neo4j.Core.Linting.Validation.Validation;

namespace SchemaSmith.Neo4j.CLI.SubCommands;

public class Lint
{
    public static readonly Command LintCommand;

    static Lint()
    {
        LintCommand = new Command(
            name: "lint",
            description: "Lint neo4j schema file."
        )
        {
            SchemaSmithFileOptions.NeoSchemaFileInfo
        };

        LintCommand.SetHandler(LintNeoSchemaAsync, SchemaSmithFileOptions.NeoSchemaFileInfo);
    }

    public static async Task LintNeoSchemaAsync(FileInfo file) =>
        await Linter.LintAsync(file, new NeoSpecValidator());
}