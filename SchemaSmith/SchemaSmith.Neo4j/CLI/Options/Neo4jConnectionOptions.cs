using System.CommandLine;

namespace SchemaSmith.Neo4j.CLI.Options;

public class Neo4jConnectionOptions
{
    public static readonly Option<Uri> ServerUrl = new Option<Uri>(
        aliases: new[] { "--server-url", "-s" },
        description: "Neo4j server url."
    )
    {
        IsRequired = true,
        Arity = ArgumentArity.ExactlyOne
    };

    public static readonly Option<string> Username = new Option<string>(
        aliases: new[] { "--username", "-u" },
        description: "Username to run scripts with."
    )
    {
        IsRequired = true,
        Arity = ArgumentArity.ExactlyOne
    };
    
    public static readonly Option<string> Password = new Option<string>(
        aliases: new[] { "--password", "-p" },
        description: "Username to run scripts with."
    )
    {
        IsRequired = true,
        Arity = ArgumentArity.ExactlyOne
    };
    
    public static readonly Option<string> DatabaseName = new Option<string>(
        aliases: new[] { "--database-name", "-d" },
        description: "Neo4j database name."
    )
    {
        IsRequired = true,
        Arity = ArgumentArity.ExactlyOne
    };
    
    public static readonly Option<string> Timeout = new Option<string>(
        aliases: new[] { "--timeout", "-t" },
        description: "Neo4j query timeout. Default is milliseconds, but can be suffixed with 'ms' for milliseconds, 's' for seconds or 'm' for minutes."
    )
    {
        IsRequired = false,
        Arity = ArgumentArity.ExactlyOne
    };
    
    private static readonly HashSet<string> ValidTimeoutUnits = new() { "ms", "s", "m" };

    public const string TimeoutStringFormattingExceptionMessage =
        "Timeout must be a valid integer, optionally suffixed with 'ms', 's' or 'm'.";
    public static int GetMillisecondsFromTimeoutArg(string timeout)
    {
        if (string.IsNullOrWhiteSpace(timeout))
            return 10000;
        
        var timeoutUnit = ValidTimeoutUnits.FirstOrDefault(timeout.EndsWith);
        
        var timeoutDurationString = timeoutUnit is null ? timeout : timeout[..^timeoutUnit.Length];

        if (!int.TryParse(timeoutDurationString, out var timeoutDurationInt))
            throw new Exception(TimeoutStringFormattingExceptionMessage);
        
        return timeoutUnit switch {
            "ms" => timeoutDurationInt,
            "s" => timeoutDurationInt * 1000,
            "m" => timeoutDurationInt * 1000 * 60,
            _ => timeoutDurationInt
        };       
    }
}