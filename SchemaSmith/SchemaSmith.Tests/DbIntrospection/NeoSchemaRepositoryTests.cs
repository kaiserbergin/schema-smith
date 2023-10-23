using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Graphr.Neo4j.Graphr;
using SchemaSmith.Domain;
using SchemaSmith.Neo4j.Infrastructure.Introspection;
using SchemaSmith.Queries.Provider;
using SchemaSmith.Tests.Fixtures;
using VerifyXunit;
using Xunit;

namespace SchemaSmith.Tests.DbIntrospection;

[UsesVerify]
public class NeoSchemaRepositoryTests : IClassFixture<Neo4jFixture>, IAsyncLifetime
{
    private readonly Neo4jFixture _neo4JFixture;
    private readonly INeoGraphr _neoGraphr;

    private readonly INeoSchemaRepository _repository;

    public NeoSchemaRepositoryTests(Neo4jFixture neo4JFixture)
    {
        _neo4JFixture = neo4JFixture;
        _neoGraphr = _neo4JFixture.NeoGraphr;

        _repository = new NeoSchemaRepository(_neo4JFixture.ConfigurationSettings);
    }

    #region Constraints

    [Fact]
    public async void GetConstraints_WithConstraints_ReturnsConstraints()
    {
        // Arrange
        var uniqueNodePropertyConstraint = "CREATE CONSTRAINT Test_UNP FOR (n:Test) REQUIRE n.unp IS UNIQUE";
        var nodePropertyExistenceConstraint = "CREATE CONSTRAINT Test_NPE FOR (n:Test) REQUIRE n.npe IS NOT NULL";
        var relationshipPropertyExistenceConstraint = "CREATE CONSTRAINT REL_RPE FOR ()-[r:REL]-() REQUIRE r.rpe IS NOT NULL";
        var nodeKeyConstraint = "CREATE CONSTRAINT Test_NK FOR (n:Test) REQUIRE n.nk IS NODE KEY";

        await _neoGraphr.WriteAsync(uniqueNodePropertyConstraint);
        await _neoGraphr.WriteAsync(nodePropertyExistenceConstraint);
        await _neoGraphr.WriteAsync(relationshipPropertyExistenceConstraint);
        await _neoGraphr.WriteAsync(nodeKeyConstraint);

        // Act
        var result = _repository.GetConstraints();

        // Assert
        await Verifier.Verify(result);
    }

    [Fact]
    public void GetConstraints_WithoutConstraints_ReturnsNoConstraints()
    {
        // Act
        var result = _repository.GetConstraints();

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region Indexs

    [Fact]
    public async void GetIndexes_WithIndexes_ReturnsIndexes()
    {
        // Arrange
        var nodeTextIndex = "CREATE TEXT INDEX Node_TEXT FOR (n:Test) ON (n.text)";
        var relationshipTextIndex = "CREATE TEXT INDEX Relationship_TEXT FOR ()-[r:REL]-() ON (r.text)";
        var nodePointIndex = "CREATE POINT INDEX Node_POINT FOR (n:Test) ON (n.location)";
        var relationshipPointIndex = "CREATE POINT INDEX Relationship_POINT FOR ()-[r:REL]-() ON (r.location)";
        var nodeRangeIndex = "CREATE RANGE INDEX Node_RANGE FOR (n:Test) ON (n.range1, n.range2)";
        var relationshipRangeIndex = "CREATE RANGE INDEX Relationship_RANGE FOR ()-[r:REL]-() ON (r.range1, r.range2)";

        await _neoGraphr.WriteAsync(nodeTextIndex);
        await _neoGraphr.WriteAsync(relationshipTextIndex);
        await _neoGraphr.WriteAsync(nodePointIndex);
        await _neoGraphr.WriteAsync(relationshipPointIndex);
        await _neoGraphr.WriteAsync(nodeRangeIndex);
        await _neoGraphr.WriteAsync(relationshipRangeIndex);

        // Act
        var result = _repository.GetIndexes();

        // Assert
        await Verifier.Verify(result);
    }
    
    [Fact]
    public void GetIndexes_WithoutIndexes_ReturnsNoIndexes()
    {
        // Act
        var result = _repository.GetIndexes();

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region Nodes and Relationships
    
    [Fact]
    public async void GetDatabaseEntities_WithNodesAndRelationships_ReturnsEntities()
    {
        // Arrange
        await _neoGraphr.WriteAsync(TestQueryProvider.PlayMovies);
        await _neoGraphr.WriteAsync(TestQueryProvider.CreateAllTypeNode);
        
        // Act
        var result = _repository.GetDatabaseEntities();

        // Assert
        await Verifier.Verify(result);
    }

    [Fact]
    public void GetDatabaseEntities_WithOutNodesOrRelationships_ReturnsNoEntities()
    {
        // Act
        var result = _repository.GetDatabaseEntities();

        // Assert
        result.Nodes.Should().BeEmpty();
        result.Relationships.Should().BeEmpty();
    }

    #endregion

    #region Server Schema

    [Fact]
    public async void GetServerSchema_WithBuiltOutDatabase_ReturnsSchema()
    {
        // Arrange
        var personIndex = "CREATE RANGE INDEX Person_BTREE FOR (n:Person) ON (n.age)";
        var personConstraint = "CREATE CONSTRAINT Person_UNP FOR (n:Person) REQUIRE n.name IS UNIQUE";
        
        await _neoGraphr.WriteAsync(TestQueryProvider.PlayMovies);
        await _neoGraphr.WriteAsync(personIndex);
        await _neoGraphr.WriteAsync(personConstraint);
        
        // Act
        var result = _repository.GetServerSchema();

        // Assert
        result.ServerUrl.Should().Be(_neo4JFixture.Neo4JTestContainer.GetConnectionString());
        await Verifier.Verify(result.Graphs);
    }

    [Fact]
    public async void GetServerSchema_WithEmptyDatabase_ReturnsSchema()
    {
        // Act
        var result = _repository.GetServerSchema();

        // Assert
        result.ServerUrl.Should().Be(_neo4JFixture.Neo4JTestContainer.GetConnectionString());
        await Verifier.Verify(result.Graphs);
    }

    #endregion

    #region Server Metadata

    [Fact]
    public async Task GetServerVersions_ReturnsVersion()
    {
        // Act
        var result = _repository.GetVersion();

        // Assert
        await Verifier.Verify(result);
    }

    #endregion

    public async Task InitializeAsync()
    {
        await _neoGraphr.WriteAsync("CALL apoc.schema.assert({}, {})");
        await _neoGraphr.WriteAsync(TestQueryProvider.Cleanup);
    }

    public async Task DisposeAsync()
    {
    }
}