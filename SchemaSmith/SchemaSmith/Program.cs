using System.CommandLine;
using SchemaSmith.Neo4j.CLI;
using SchemaSmith.Neo4j.CLI.SubCommands;
using SchemaSmith.Neo4j.Domain.Dto;

namespace SchemaSmith;

internal class Program
{
    internal static ServerSchema? ServerSchema;
    internal static FileInfo? CypherFile;
    static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;

        var rootCommand = new RootCommand("Sample app for System.CommandLine");
        
        rootCommand.AddCommand(Neo4jCommandProvider.Neo4jCommand);

        return await rootCommand.InvokeAsync(args);
    }
}