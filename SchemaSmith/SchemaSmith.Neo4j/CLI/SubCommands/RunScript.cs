using System.CommandLine;
using System.Text;
using Graphr.Neo4j.Configuration;
using Graphr.Neo4j.Driver;
using Graphr.Neo4j.Graphr;
using Graphr.Neo4j.QueryExecution;
using SchemaSmith.Neo4j.CLI.Options;

namespace SchemaSmith.Neo4j.CLI.SubCommands;

public class RunScript
{
    public static readonly Command RunScriptCommand;

    static RunScript()
    {
        RunScriptCommand = new Command(
            name: "run-script",
            description: "Runs cypher script at the desired location.")
        {
            SchemaSmithFileOptions.CypherScriptInfo,
            Neo4jConnectionOptions.ServerUrl,
            Neo4jConnectionOptions.Username,
            Neo4jConnectionOptions.Password,
            Neo4jConnectionOptions.Timeout
        };

        RunScriptCommand.SetHandler(
            RunCypherAsync,
            SchemaSmithFileOptions.CypherScriptInfo,
            Neo4jConnectionOptions.ServerUrl,
            Neo4jConnectionOptions.Username,
            Neo4jConnectionOptions.Password,
            Neo4jConnectionOptions.Timeout
        );
    }

    private static async Task RunCypherAsync(
        FileInfo cypherScript,
        Uri serverUrl,
        string username,
        string password,
        string timeout)
    {
        var settings = new NeoDriverConfigurationSettings
        {
            Url = serverUrl.ToString(),
            Username = username,
            Password = password,
            VerifyConnectivity = true,
            QueryTimeoutInMs = Neo4jConnectionOptions.GetMillisecondsFromTimeoutArg(timeout)
        };

        var driverProvider = new DriverProvider(settings);
        var queryExecutor = new QueryExecutor(driverProvider, settings);

        var sb = new StringBuilder();
        string? dbName = null;

        File.ReadLines(cypherScript.FullName)
            .ToList()
            .ForEach(async line =>
            {
                var useDbPosition = line.IndexOf(":use ", StringComparison.Ordinal);

                if (useDbPosition > -1)
                {
                    dbName = line[5..^1];
                    sb.Clear();
                    return;
                }

                sb.Append(line);
                sb.AppendLine();

                if (!line.Contains(';')) 
                    return;

                if (dbName == null)
                    throw new ArgumentNullException("Database name not found");
                
                Console.WriteLine(sb.ToString());

                var neoGraphr = new NeoGraphr(queryExecutor)
                    .WithSessionConfig(builder => builder.WithDatabase(dbName));

                await neoGraphr.WriteAsync(sb.ToString());
                
                sb.Clear();
            });
    }
}