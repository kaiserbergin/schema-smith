using System.CommandLine;
using SchemaSmith.Infrastructure.IO;
using SchemaSmith.SQLServer.CLI.Options;
using SchemaSmith.SQLServer.Core.ScriptGeneration;
using SchemaSmith.SQLServer.Domain;

namespace SchemaSmith.SQLServer.CLI.Subcommands;

public class ScriptCommandProvider
{
    public static readonly Command Command;

    static ScriptCommandProvider()
    {
        Command = new Command(
            name: "script",
            description: "Script out the cypher statements."
        )
        {
            SqlServerSchemaFileOptions.SchemaFileSourceFileInfo,
            SqlServerSchemaFileOptions.OutputScriptFileInfo,
        };

        Command.SetHandler(
            ScriptCypher, 
            SqlServerSchemaFileOptions.SchemaFileSourceFileInfo,
            SqlServerSchemaFileOptions.OutputScriptFileInfo
            );
    }

    public static async Task ScriptCypher(FileInfo file, FileInfo? outputFileInfo)
    {
        Console.ResetColor();

        // TODO: Make a linter bruh
        // await Lint.LintNeoSchemaAsync(file);

        var serverSchema = SpecReader.GetServerSchemaFromPath<Database>(file.FullName);

        var schemaGenerator = new SqlServerScriptGenerator();
        
        var sqlStatements = schemaGenerator.Generate(serverSchema);
        
        if (!outputFileInfo.Exists)
            Directory.CreateDirectory(outputFileInfo.Directory!.FullName);
        
        using var outputFile = File.Open(outputFileInfo!.FullName, FileMode.Create);
        using var streamWriter = new StreamWriter(outputFile);
        
        await streamWriter.WriteLineAsync(sqlStatements);
        
        streamWriter.Close();
        outputFile.Close();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  \x2714  Sql script generated at: {outputFileInfo.FullName}!");
        
        // TODO: Add verbosity stuff.
        // Console.ForegroundColor = ConsoleColor.White;
        // File.ReadLines(outputFileInfo.FullName)
        //     .ToList()
        //     .ForEach(Console.WriteLine);

        Console.ResetColor();
    }
}