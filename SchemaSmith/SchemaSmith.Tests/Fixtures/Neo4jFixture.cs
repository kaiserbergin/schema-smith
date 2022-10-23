using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Graphr.Neo4j.Configuration;
using Graphr.Neo4j.Driver;
using Graphr.Neo4j.Graphr;
using Graphr.Neo4j.QueryExecution;
using Neo4j.Driver;
using Xunit;

namespace SchemaSmith.Tests.Fixtures;

public class Neo4jFixture : IAsyncLifetime
{
    public readonly TestcontainerDatabaseConfiguration Configuration = new Neo4jTestcontainerConfiguration { Password = "password" };
    public readonly Neo4jTestcontainer Neo4JTestcontainer;
    
    public NeoDriverConfigurationSettings ConfigurationSettings => new NeoDriverConfigurationSettings
    {
        Url = Neo4JTestcontainer.ConnectionString,
        Username = Neo4JTestcontainer.Username,
        Password = Neo4JTestcontainer.Password,
        DatabaseName = Neo4JTestcontainer.Database,
        QueryTimeoutInMs = 10000
    };

    public INeoGraphr NeoGraphr => new NeoGraphr(
        new QueryExecutor(
            new DriverProvider(ConfigurationSettings),
            ConfigurationSettings)
    );

    public Neo4jFixture()
    {
        Neo4JTestcontainer = new TestcontainersBuilder<Neo4jTestcontainer>()
            .WithDatabase(this.Configuration)
            .WithImage("neo4j:enterprise")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await Neo4JTestcontainer.StartAsync()
            .ConfigureAwait(false);
    }

    public async Task DisposeAsync()
    {
        await Neo4JTestcontainer.DisposeAsync()
                 .ConfigureAwait(false);
        Configuration.Dispose();
    }
}