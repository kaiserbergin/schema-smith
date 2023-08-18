using System.Threading.Tasks;
using Graphr.Neo4j.Configuration;
using Graphr.Neo4j.Driver;
using Graphr.Neo4j.Graphr;
using Graphr.Neo4j.QueryExecution;
using Testcontainers.Neo4j;
using Xunit;

namespace SchemaSmith.Tests.Fixtures;

public class Neo4jFixture : IAsyncLifetime
{
    public readonly Neo4jContainer Neo4JTestContainer;
    
    private const string _username = "neo4j";
    private const string _password = "connect";
    private const string _databaseName = "neo4j";

    public NeoDriverConfigurationSettings ConfigurationSettings => new()
    {
        Url = Neo4JTestContainer.GetConnectionString(),
        Username = _username,
        Password = _password,
        DatabaseName = _databaseName,
        QueryTimeoutInMs = 10000
    };

    public INeoGraphr NeoGraphr => new NeoGraphr(
        new QueryExecutor(
            new DriverProvider(ConfigurationSettings),
            ConfigurationSettings)
    );

    public Neo4jFixture()
    {
        Neo4JTestContainer = new Neo4jBuilder()
            .WithImage("neo4j:enterprise")
            .WithEnvironment("NEO4J_ACCEPT_LICENSE_AGREEMENT", "yes")
            .WithEnvironment("NEO4J_PLUGINS", "[\"apoc\"]")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await Neo4JTestContainer.StartAsync()
            .ConfigureAwait(false);
    }

    public async Task DisposeAsync()
    {
        await Neo4JTestContainer.DisposeAsync()
            .ConfigureAwait(false);
    }
}