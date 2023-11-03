using System.Reflection;

namespace SchemaSmith.Neo4j.Infrastructure.Queries.Provider;

public static class QueryProvider
{
    public static readonly string DeleteSchemaSmithEntities;
    public static readonly string ShowConstraints;
    public static readonly string ShowIndexes;
    public static readonly string ShowMetaSchema;
    public static readonly string ShowPrivileges;
    public static readonly string GetVersions;

    private const string INFRA_DIR = @"/Infrastructure";
    private const string QUERY_DIR = @"/Queries";
    private const string DB_INSPECTION_DIR = @"/DbInspection";
    private const string CLEANUP_DIR = @"/Cleanup";

    private static readonly string _buildDir;
    private static readonly string _queryDir;

    static QueryProvider()
    {
        _buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _queryDir = $@"{_buildDir}{INFRA_DIR}{QUERY_DIR}";

        DeleteSchemaSmithEntities = File.ReadAllText(@$"{_queryDir}{CLEANUP_DIR}/delete-schema-smith-entities.cypher");
        ShowConstraints = File.ReadAllText(@$"{_queryDir}{DB_INSPECTION_DIR}/show-constraints.cypher");
        ShowIndexes = File.ReadAllText(@$"{_queryDir}{DB_INSPECTION_DIR}/show-indexes.cypher");
        ShowMetaSchema = File.ReadAllText(@$"{_queryDir}{DB_INSPECTION_DIR}/show-meta-schema.cypher");
        ShowPrivileges = File.ReadAllText(@$"{_queryDir}{DB_INSPECTION_DIR}/show-privileges.cypher");
        GetVersions = File.ReadAllText(@$"{_queryDir}{DB_INSPECTION_DIR}/get-versions.cypher");
    }
}