using System.CommandLine;
using SchemaSmith.SQLServer.CLI.Subcommands;

namespace SchemaSmith.SQLServer.CLI;

public class SqlServerCommandProvider
{
    public static readonly Command SqlServerCommand;

    static SqlServerCommandProvider()
    {
        SqlServerCommand = new Command(
            name: "sqlserver",
            description: "SqlServer commands."
        );

        SqlServerCommand.AddCommand(ScriptCommandProvider.Command);
        SqlServerCommand.AddCommand(DocumentCommandProvider.Command);
    }
}