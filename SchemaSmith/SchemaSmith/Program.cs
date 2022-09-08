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
        
        var fileOption = new Option<FileInfo?>(
            name: "--file",
            description: "The file to read and display on the console.");
        
        var delayOption = new Option<int>(
            aliases: new[] {"--delay", "-d" },
            description: "Delay between lines, specified as milliseconds per character in a line.",
            getDefaultValue: () => 42);

        var fgcolorOption = new Option<ConsoleColor>(
            aliases: new[] {"--fgcolor", "-c" },
            description: "Foreground color of text displayed on the console.",
            getDefaultValue: () => ConsoleColor.White);

        var lightModeOption = new Option<bool>(
            aliases: new[] {"--light-mode", "-l" },
            description: "Background color of text displayed on the console: default is black, light mode is white.");

        var rootCommand = new RootCommand("Sample app for System.CommandLine");

        var readCommand = new Command("read", "Read and display the file")
        {
            fileOption,
            delayOption,
            fgcolorOption,
            lightModeOption
        };
        
        rootCommand.AddCommand(readCommand);
        rootCommand.AddCommand(Lint.LintCommand);
        rootCommand.AddCommand(Script.ScriptCommand);
        rootCommand.AddCommand(RunScript.RunScriptCommand);
        
        readCommand.SetHandler(async (file, delay, fgcolor, lightMode) =>
            {
                await ReadFile(file!, delay, fgcolor, lightMode);
            },
            fileOption, delayOption, fgcolorOption, lightModeOption);

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