using System.Reflection;

namespace SchemaSmith.Queries.Provider;

internal static class QueryProvider
{
    internal static readonly string DeleteSchemaSmithEntities;
    internal static readonly string ShowConstraints;
    internal static readonly string ShowIndexes;
    internal static readonly string ShowMetaSchema;
    internal static readonly string ShowPrivileges;
    internal static readonly string GetVersions;

    private const string DB_INSPECTION_DIR = @"/DbInspection";
    private const string CLEANUP_DIR = @"/Cleanup";

    private static readonly string _buildDir;
    private static readonly string _queryDir;

    static QueryProvider()
    {
        _buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _queryDir = $@"{_buildDir}/Queries";

        DeleteSchemaSmithEntities = File.ReadAllText(@$"{_queryDir}{CLEANUP_DIR}/delete-schema-smith-entities.cypher");
        ShowConstraints = File.ReadAllText(@$"{_queryDir}{DB_INSPECTION_DIR}/show-constraints.cypher");
        ShowIndexes = File.ReadAllText(@$"{_queryDir}{DB_INSPECTION_DIR}/show-indexes.cypher");
        ShowMetaSchema = File.ReadAllText(@$"{_queryDir}{DB_INSPECTION_DIR}/show-meta-schema.cypher");
        ShowPrivileges = File.ReadAllText(@$"{_queryDir}{DB_INSPECTION_DIR}/show-privileges.cypher");
        GetVersions = File.ReadAllText(@$"{_queryDir}{DB_INSPECTION_DIR}/get-versions.cypher");
    }
}