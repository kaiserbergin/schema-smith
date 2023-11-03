using System.CommandLine;

namespace SchemaSmith.Neo4j.CLI.Options;

public class SchemaSmithFileOptions
{
    public static readonly Option<FileInfo> NeoSchemaFileInfo = new Option<FileInfo>(
                aliases: new[] { "--file", "-f" },
                description: "Neo4j schema file path.")
        {
            Arity = ArgumentArity.ExactlyOne,
            IsRequired = true
        }
        .ExistingOnly()
        .LegalFilePathsOnly();
        

    public static readonly Option<FileInfo?> OutputCypherInfo = new Option<FileInfo?>(
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
        {
            Arity = ArgumentArity.ZeroOrOne
        }
        .LegalFilePathsOnly();
    
    public static readonly Option<FileInfo?> OutputSchemaSmithYamlInfo = new Option<FileInfo?>(
            aliases: new[] { "--output", "-o" },
            description: "Output schemasmith yaml file",
            getDefaultValue: () =>
            {
                var date = DateTime.UtcNow;
                var fileInfo = new FileInfo($"./schema-smith-specs/{date:yyyy_MM_dd_T_HH_mm_ss}__SchemaSmithSpec.yaml");
                
                // TODO: Validation for directory stuff.
                if (!fileInfo.Exists)
                    Directory.CreateDirectory(fileInfo.Directory!.FullName);

                return fileInfo;
            })
        {
            Arity = ArgumentArity.ZeroOrOne
        }
        .LegalFilePathsOnly();

    public static readonly Option<FileInfo> CypherScriptInfo = new Option<FileInfo>(
            aliases: new[] { "--file", "-f" },
            description: "Cypher script file path.")
        {
            IsRequired = true,
            Arity = ArgumentArity.ExactlyOne
        }
        .ExistingOnly()
        .LegalFilePathsOnly();

}