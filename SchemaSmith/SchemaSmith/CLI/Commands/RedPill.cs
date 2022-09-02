using System.CommandLine;
using SchemaSmith.CLI.Options;

namespace SchemaSmith.CLI.Commands;

public class RedPill
{
    internal static readonly Command RedPillCommand;

    static RedPill()
    {
        RedPillCommand = new Command(
            name: "red-pill",
            description: "Lint, generate scripts, and execute scripts against neo4j"
        )
        {
            SchemaSmithFileOptions.NeoSchemaFileInfo,
            SchemaSmithFileOptions.OutputCypherInfo,
            
        };
    }
}