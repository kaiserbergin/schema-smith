using System.CommandLine;
using SchemaSmith.Infrastructure.IO;
using SchemaSmith.SQLServer.CLI.Options;
using SchemaSmith.SQLServer.Core.DocumentationGeneration;
using SchemaSmith.SQLServer.Core.ScriptGeneration;
using SchemaSmith.SQLServer.Domain;

namespace SchemaSmith.SQLServer.CLI.Subcommands;

public class DocumentCommandProvider
{
    public static readonly Command Command;

    static DocumentCommandProvider()
    {
        Command = new Command(
            name: "document",
            description: "Generate documentation."
        )
        {
            SqlServerSchemaFileOptions.SchemaFileSourceFileInfo,
            SqlServerSchemaFileOptions.OutputDocumentationFileInfo,
        };

        Command.SetHandler(
            ScriptCypher, 
            SqlServerSchemaFileOptions.SchemaFileSourceFileInfo,
            SqlServerSchemaFileOptions.OutputDocumentationFileInfo
            );
    }

    public static async Task ScriptCypher(FileInfo file, FileInfo? outputFileInfo)
    {
        Console.ResetColor();

        // TODO: Make a linter bruh
        // await Lint.LintNeoSchemaAsync(file);

        var serverSchema = SpecReader.GetServerSchemaFromPath<Database>(file.FullName);

        var documentGenerator = new SqlServerDocumentationGenerator();
        
        var sqlStatements = documentGenerator.Generate(serverSchema);
        
        if (!outputFileInfo.Exists)
            Directory.CreateDirectory(outputFileInfo.Directory!.FullName);
        
        using var outputFile = File.Open(outputFileInfo!.FullName, FileMode.Create);
        using var streamWriter = new StreamWriter(outputFile);
        
        await streamWriter.WriteLineAsync(sqlStatements);
        
        streamWriter.Close();
        outputFile.Close();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  \x2714  SqlServer documentation generated at: {outputFileInfo.FullName}!");
        
        // TODO: Add verbosity stuff.
        // Console.ForegroundColor = ConsoleColor.White;
        // File.ReadLines(outputFileInfo.FullName)
        //     .ToList()
        //     .ForEach(Console.WriteLine);

        Console.ResetColor();
    }
}