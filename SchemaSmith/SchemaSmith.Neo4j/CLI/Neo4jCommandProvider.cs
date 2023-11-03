using System.CommandLine;
using SchemaSmith.Neo4j.CLI.SubCommands;

namespace SchemaSmith.Neo4j.CLI;

public class Neo4jCommandProvider
{
    public static readonly Command Neo4jCommand;

    static Neo4jCommandProvider()
    {
        Neo4jCommand = new Command(
            name: "neo4j",
            description: "Neo4j commands."
        );

        Neo4jCommand.AddCommand(Lint.LintCommand);
        Neo4jCommand.AddCommand(Script.ScriptCommand);
        Neo4jCommand.AddCommand(Introspect.IntrospectCommand);
        Neo4jCommand.AddCommand(RunScript.RunScriptCommand);
    }
}