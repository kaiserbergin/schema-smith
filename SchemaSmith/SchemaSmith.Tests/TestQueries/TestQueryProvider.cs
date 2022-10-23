using System.IO;
using System.Reflection;

namespace SchemaSmith.Queries.Provider;

internal static class TestQueryProvider
{
    internal static readonly string PlayMovies;
    internal static readonly string CreateAllTypeNode;
    internal static readonly string Cleanup;

    private static readonly string _buildDir;
    private static readonly string _queryDir;

    static TestQueryProvider()
    {
        _buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _queryDir = $@"{_buildDir}/TestQueries";

        PlayMovies = File.ReadAllText(@$"{_queryDir}/play-movies.cypher");
        CreateAllTypeNode = File.ReadAllText(@$"{_queryDir}/create-all-type.cypher");
        Cleanup = File.ReadAllText(@$"{_queryDir}/cleanup.cypher");
    }
}