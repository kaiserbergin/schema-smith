using System.CommandLine;

namespace SchemaSmith.CLI.Options;

public class Neo4jConnectionOptions
{
    internal static readonly Option<Uri> ServerUrl = new Option<Uri>(
        aliases: new[] { "--server-url", "-s" },
        description: "Neo4j server url."
    )
    {
        Arity = ArgumentArity.ExactlyOne
    };

    internal static readonly Option<string> Username = new Option<string>(
        aliases: new[] { "--username", "-u" },
        description: "Username to run scripts with."
    )
    {
        Arity = ArgumentArity.ExactlyOne
    };
    
    internal static readonly Option<string> Password = new Option<string>(
        aliases: new[] { "--password", "-p" },
        description: "Username to run scripts with."
    )
    {
        Arity = ArgumentArity.ExactlyOne
    };
}