using System.CommandLine;
using SchemaSmith.Infrastructure.IO;
using SchemaSmith.Neo4j.CLI.Options;
using SchemaSmith.Neo4j.Core.ScriptGeneration;
using SchemaSmith.Neo4j.Domain.Dto;

namespace SchemaSmith.Neo4j.CLI.SubCommands;

public class Script
{
    public static readonly Command ScriptCommand;

    static Script()
    {
        ScriptCommand = new Command(
            name: "script",
            description: "Script out the cypher statements."
        )
        {
            SchemaSmithFileOptions.NeoSchemaFileInfo,
            SchemaSmithFileOptions.OutputCypherInfo,
        };

        ScriptCommand.SetHandler(
            ScriptCypher, 
            SchemaSmithFileOptions.NeoSchemaFileInfo,
            SchemaSmithFileOptions.OutputCypherInfo
            );
    }

    public static async Task ScriptCypher(FileInfo file, FileInfo? outputFileInfo)
    {
        Console.ResetColor();

        await Lint.LintNeoSchemaAsync(file);

        var serverSchema = SpecReader.GetServerSchemaFromPath<ServerSchema>(file.FullName);

        var schemaGenerator = new CreateScriptGenerator();
        
        var cypherStatements = serverSchema
            .Graphs
            .Select(schema => schemaGenerator.Generate(schema))
            .ToList();
        
        if (!outputFileInfo.Exists)
            Directory.CreateDirectory(outputFileInfo.Directory!.FullName);
        
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