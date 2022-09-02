using System.CommandLine;

namespace SchemaSmith.CLI.Options;

internal class SchemaFileOptions
{
    internal static readonly Option<FileInfo> NeoSchemaFileInfo = new Option<FileInfo>(
            aliases: new[] { "--file", "-f" },
            description: "Neo4j schema file path.")
        .ExistingOnly()
        .LegalFilePathsOnly();
}