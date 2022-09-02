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
            SchemaFileOptions.NeoSchemaFileInfo,
            SchemaFileOptions.OutputCypherInfo,
        };

        ScriptCommand.SetHandler(
            ScriptCypher, 
            SchemaFileOptions.NeoSchemaFileInfo,
            SchemaFileOptions.OutputCypherInfo
            );
    }

    internal static void ScriptCypher(FileInfo file, FileInfo? outputFileInfo)
    {
        Console.ResetColor();

        Lint.LintNeoSchema(file);

        var serverSchema = SpecReader.GetServerSchemaFromPath(file.FullName);
        var cypherStatements = serverSchema
            .Graphs
            .SelectMany(schema => schema.GenerateCypherStatements())
            .ToList();

        // The type is optional because it is a parameter for 
        //  the command line, but we have a default value set
        //  for it regardless.
        using var outputFile = File.Open(outputFileInfo!.FullName, FileMode.Create);
        using var streamWriter = new StreamWriter(outputFile);
        
        cypherStatements.ForEach(streamWriter.WriteLine);
        
        streamWriter.Close();
        outputFile.Close();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  \x2714  Cypher script generated at: {outputFileInfo.FullName}!");
        
        // TODO: Add verbosity stuff.
        // Console.ForegroundColor = ConsoleColor.White;
        // File.ReadLines(outputFileInfo.FullName)
        //     .ToList()
        //     .ForEach(Console.WriteLine);

        Console.ResetColor();
    }
}