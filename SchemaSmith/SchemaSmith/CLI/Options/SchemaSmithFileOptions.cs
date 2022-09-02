using System.CommandLine;
using System.Globalization;

namespace SchemaSmith.CLI.Options;

internal class SchemaSmithFileOptions
{
    internal static readonly Option<FileInfo> NeoSchemaFileInfo = new Option<FileInfo>(
            aliases: new[] { "--file", "-f" },
            description: "Neo4j schema file path.")
        .ExistingOnly()
        .LegalFilePathsOnly();

    internal static readonly Option<FileInfo?> OutputCypherInfo = new Option<FileInfo?>(
            aliases: new[] { "--output", "-o" },
            description: "Output cypher file",
            getDefaultValue: () =>
            {
                var date = DateTime.UtcNow;
                var fileInfo = new FileInfo($"./cypher-scripts/{date:yyyy_MM_dd_T_HH_mm_ss}__SchemaSmith.cypher");
                
                // TODO: Validation for directory stuff.
                if (!fileInfo.Exists)
                    Directory.CreateDirectory(fileInfo.Directory!.FullName);

                return fileInfo;
            })
        .LegalFilePathsOnly();

    internal static readonly Option<FileInfo> CypherScriptInfo = new Option<FileInfo>(
            aliases: new[] { "--file", "-f" },
            description: "Cypher script file path.")
        {
            Arity = ArgumentArity.ExactlyOne
        }
        .ExistingOnly()
        .LegalFilePathsOnly();

}