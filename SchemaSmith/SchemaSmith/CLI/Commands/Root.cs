using System.CommandLine;

namespace SchemaSmith.CLI.Commands;

internal class Root
{
    internal static readonly RootCommand RootCommand = new (
        "CLI for generating / documenting neo4j schemas / scripts."
    );
}