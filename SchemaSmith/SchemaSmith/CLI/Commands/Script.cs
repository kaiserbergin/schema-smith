using System.CommandLine;
using SchemaSmith.CLI.Options;
using SchemaSmith.CypherStatementExtensions;
using SchemaSmith.IO;

namespace SchemaSmith.CLI.Commands;

internal class Script
{
    internal static readonly Command ScriptCommand;

    static Script()
    {
        ScriptCommand = new Command(
            name: "script",
            description: "Script out the cypher statements"
        )
        {
            SchemaFileOptions.NeoSchemaFileInfo
        };

        ScriptCommand.SetHandler(ScriptCypher, SchemaFileOptions.NeoSchemaFileInfo);
    }

    internal static void ScriptCypher(FileInfo file)
    {
        Console.ResetColor();

        Lint.LintNeoSchema(file);

        var serverSchema = SpecReader.GetServerSchemaFromPath(file.FullName);
        var cypherStatements = serverSchema
            .Graphs
            .SelectMany(schema => schema.GenerateCypherStatements())
            .ToList();

        cypherStatements.ForEach(Console.WriteLine);

        Console.ResetColor();
    }
}