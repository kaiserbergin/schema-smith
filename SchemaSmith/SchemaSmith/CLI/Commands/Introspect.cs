using System.CommandLine;
using Graphr.Neo4j.Configuration;
using Graphr.Neo4j.Driver;
using Graphr.Neo4j.Graphr;
using Graphr.Neo4j.QueryExecution;
using SchemaSmith.CLI.Options;
using SchemaSmith.DbIntrospection;
using SchemaSmith.IO;
using SchemaSmith.Queries.Provider;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SchemaSmith.CLI.Commands;

internal class Introspect
{
    internal static readonly Command IntrospectCommand;

    static Introspect()
    {
        IntrospectCommand = new Command(
            name: "introspect",
            description: "Generate schemasmith yaml from existing database"
        )
        {
            Neo4jConnectionOptions.ServerUrl,
            Neo4jConnectionOptions.Username,
            Neo4jConnectionOptions.Password,
            Neo4jConnectionOptions.DatabaseName,
            SchemaSmithFileOptions.OutputSchemaSmithYamlInfo
        };

        IntrospectCommand.SetHandler(
            GenerateYaml,
            Neo4jConnectionOptions.ServerUrl,
            Neo4jConnectionOptions.Username,
            Neo4jConnectionOptions.Password,
            Neo4jConnectionOptions.DatabaseName,
            SchemaSmithFileOptions.OutputSchemaSmithYamlInfo
        );
    }

    private static void GenerateYaml(
        Uri serverUrl,
        string username,
        string password,
        string databaseName,
        FileInfo outputFileInfo
    )
    {
        var settings = new NeoDriverConfigurationSettings
        {
            Url = serverUrl.ToString(),
            Username = username,
            Password = password,
            DatabaseName = databaseName,
            VerifyConnectivity = true,
            QueryTimeoutInMs = 10000
        };
        
        var schemaRepository = new NeoSchemaRepository(settings);

        var schema = schemaRepository.GetServerSchema();

        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeConverter(new YamlStringEnumConverter())
            .Build();

        var yaml = serializer.Serialize(schema);
        
        if (!outputFileInfo.Exists)
            Directory.CreateDirectory(outputFileInfo.Directory!.FullName);
        
        using var outputFile = File.Open(outputFileInfo!.FullName, FileMode.Create);
        using var streamWriter = new StreamWriter(outputFile);
        
        streamWriter.Write(yaml);
        
        streamWriter.Close();
        outputFile.Close();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"SchemaSmith yaml generated at: {outputFileInfo.FullName}!");

        Console.ResetColor();
    }
}