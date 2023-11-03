using System.CommandLine;
using SchemaSmith.Core.Linting;
using SchemaSmith.Domain.Dto.Validation;
using SchemaSmith.Infrastructure.IO;
using SchemaSmith.Neo4j.CLI.Options;
using SchemaSmith.Neo4j.Core.Linting.Validation.Validation;
using SchemaSmith.Neo4j.Core.Linting.Validation.YamlNodeValidationDefinitions;

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

    public static async Task LintNeoSchemaAsync(FileInfo file)
    {
        var root = SpecReader.GetYamlMapping(file.FullName);
        await Linter.LintAsync(root, new NeoSpecValidator());
    }
}