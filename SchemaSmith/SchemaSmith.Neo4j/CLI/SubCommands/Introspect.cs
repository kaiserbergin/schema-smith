using System.CommandLine;
using Graphr.Neo4j.Configuration;
using SchemaSmith.Infrastructure.IO;
using SchemaSmith.Neo4j.CLI.Options;
using SchemaSmith.Neo4j.Infrastructure.Introspection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SchemaSmith.Neo4j.CLI.SubCommands;

public class Introspect
{
    public static readonly Command IntrospectCommand;

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
            Neo4jConnectionOptions.Timeout,
            SchemaSmithFileOptions.OutputSchemaSmithYamlInfo
        };

        IntrospectCommand.SetHandler(
            GenerateYamlAsync!,
            Neo4jConnectionOptions.ServerUrl,
            Neo4jConnectionOptions.Username,
            Neo4jConnectionOptions.Password,
            Neo4jConnectionOptions.DatabaseName,
            Neo4jConnectionOptions.Timeout,
            SchemaSmithFileOptions.OutputSchemaSmithYamlInfo
        );
    }

    private static async Task GenerateYamlAsync(
        Uri serverUrl,
        string username,
        string password,
        string databaseName,
        string timeout,
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
            QueryTimeoutInMs = Neo4jConnectionOptions.GetMillisecondsFromTimeoutArg(timeout)
        };
        
        var schemaRepository = new NeoSchemaRepository(settings);

        var schema = await schemaRepository.GetServerSchemaAsync();

        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeConverter(new YamlStringEnumConverter())
            .Build();

        var yaml = serializer.Serialize(schema);
        
        if (!outputFileInfo.Exists)
            Directory.CreateDirectory(outputFileInfo.Directory!.FullName);

        await using var outputFile = File.Open(outputFileInfo!.FullName, FileMode.Create);
        await using var streamWriter = new StreamWriter(outputFile);
        
        await streamWriter.WriteAsync(yaml);
        
        streamWriter.Close();
        outputFile.Close();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"SchemaSmith yaml generated at: {outputFileInfo.FullName}!");

        Console.ResetColor();
    }
}