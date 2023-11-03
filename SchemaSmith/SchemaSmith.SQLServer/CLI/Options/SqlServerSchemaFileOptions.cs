using System.CommandLine;

namespace SchemaSmith.SQLServer.CLI.Options;

public class SqlServerSchemaFileOptions
{
    public static readonly Option<FileInfo> SchemaFileSourceFileInfo = new Option<FileInfo>(
                aliases: new[] { "--file", "-f" },
                description: "SqlServer schema file path.")
        {
            Arity = ArgumentArity.ExactlyOne,
            IsRequired = true
        }
        .ExistingOnly()
        .LegalFilePathsOnly();
        

    public static readonly Option<FileInfo?> OutputScriptFileInfo = new Option<FileInfo?>(
            aliases: new[] { "--output", "-o" },
            description: "Output sql file",
            getDefaultValue: () =>
            {
                var date = DateTime.UtcNow;
                var fileInfo = new FileInfo($"./sql-scripts/{date:yyyy_MM_dd_T_HH_mm_ss}__SchemaSmith.sql");
                
                // TODO: Validation for directory stuff.
                if (!fileInfo.Exists)
                    Directory.CreateDirectory(fileInfo.Directory!.FullName);

                return fileInfo;
            })
        {
            Arity = ArgumentArity.ZeroOrOne
        }
        .LegalFilePathsOnly();
    
    public static readonly Option<FileInfo?> OutputDocumentationFileInfo = new Option<FileInfo?>(
            aliases: new[] { "--output", "-o" },
            description: "Output generated documentation file",
            getDefaultValue: () =>
            {
                var date = DateTime.UtcNow;
                var fileInfo = new FileInfo($"./schema-smith-docs/{date:yyyy_MM_dd_T_HH_mm_ss}__SqlServerDocumentation.md");
                
                // TODO: Validation for directory stuff.
                if (!fileInfo.Exists)
                    Directory.CreateDirectory(fileInfo.Directory!.FullName);

                return fileInfo;
            })
        {
            Arity = ArgumentArity.ZeroOrOne
        }
        .LegalFilePathsOnly();
}