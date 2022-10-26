using System.CommandLine;
using SchemaSmith.CLI.Commands;
using SchemaSmith.Domain;

namespace SchemaSmith;

internal class Program
{
    internal static ServerSchema? ServerSchema;
    internal static FileInfo? CypherFile;
    static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;

        var rootCommand = new RootCommand("Sample app for System.CommandLine");
        
        rootCommand.AddCommand(Lint.LintCommand);
        rootCommand.AddCommand(Script.ScriptCommand);
        rootCommand.AddCommand(RunScript.RunScriptCommand);
        rootCommand.AddCommand(Introspect.IntrospectCommand);

        return await rootCommand.InvokeAsync(args);
    }
}