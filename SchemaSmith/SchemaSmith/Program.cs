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

        return await rootCommand.InvokeAsync(args);
    }

    internal static async Task ReadFile(
        FileInfo file, int delay, ConsoleColor fgColor, bool lightMode)
    {
        Console.BackgroundColor = lightMode ? ConsoleColor.White : ConsoleColor.Black;
        Console.ForegroundColor = fgColor;
        List<string> lines = File.ReadLines(file.FullName).ToList();
        
        foreach (string line in lines)
        {
            Console.WriteLine(line);
            await Task.Delay(delay * line.Length);
        }
    }
}