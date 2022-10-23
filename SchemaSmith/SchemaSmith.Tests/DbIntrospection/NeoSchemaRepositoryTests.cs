using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Graphr.Neo4j.Graphr;
using SchemaSmith.DbIntrospection;
using SchemaSmith.Domain;
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

    #region ServerSchema

    

    #endregion

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
        var nodeBtreeIndex = "CREATE BTREE INDEX Node_BTREE FOR (n:Test) ON (n.tree1, n.tree2)";
        var relationshipBtreeIndex = "CREATE BTREE INDEX Relationship_BTREE FOR ()-[r:REL]-() ON (r.tree1, r.tree2)";
        var nodeTextIndex = "CREATE TEXT INDEX Node_TEXT FOR (n:Test) ON (n.text)";
        var relationshipTextIndex = "CREATE TEXT INDEX Relationship_TEXT FOR ()-[r:REL]-() ON (r.text)";
        var nodePointIndex = "CREATE POINT INDEX Node_POINT FOR (n:Test) ON (n.location)";
        var relationshipPointIndex = "CREATE POINT INDEX Relationship_POINT FOR ()-[r:REL]-() ON (r.location)";
        var nodeRangeIndex = "CREATE RANGE INDEX Node_RANGE FOR (n:Test) ON (n.range1, n.range2)";
        var relationshipRangeIndex = "CREATE RANGE INDEX Relationship_RANGE FOR ()-[r:REL]-() ON (r.range1, r.range2)";

        await _neoGraphr.WriteAsync(nodeBtreeIndex);
        await _neoGraphr.WriteAsync(relationshipBtreeIndex);
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

    #region Nodes
    
    [Fact]
    public async void GetNodes_WithNodes_ReturnsNodes()
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
    public void GetNodes_WithoutNodes_ReturnsNoNodes()
    {
        // Act
        var result = _repository.GetDatabaseEntities();

        // Assert
        result.Nodes.Should().BeEmpty();
        result.Relationships.Should().BeEmpty();
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