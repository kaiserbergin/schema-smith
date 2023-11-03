namespace SchemaSmith.SQLServer.Core.ScriptGeneration;

public class ScriptTemplates
{
    public const string ObliterateText =
        """
        /*
        =============================================
        Dropping all tables and schemas
        =============================================
        */
        
        """;

    public const string RunScriptsHeader =
        """
        /*
        =============================================
        Running custom scripts
        =============================================
        */

        """;

    public static string CreateSchemaHeader(string schemaName) =>
        $"""
        /*
        =============================================
        Creating schema '{schemaName}' and tables
        =============================================
        */
        
        """;
    
    public const string FKScriptHeader =
        """
        /*
        =============================================
        Creating Foreign Keys
        =============================================
        */

        """;
    
    public const string ExtendedPropertiesHeader =
        """
        /*
        =============================================
        Creating Extended Properties
        =============================================
        */

        """;

    public static string DropAllFKs(string schemaName) =>
        $"""
         IF EXISTS (SELECT * FROM sys.schemas WHERE name = '{schemaName}')
         BEGIN
             DECLARE @SchemaName NVARCHAR(128)
             SET @SchemaName = '{schemaName}'
             
             DECLARE @SQL NVARCHAR(MAX) = ''
             
             -- Drop FOREIGN KEY constraints
             SELECT @SQL = @SQL + 'ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + ' DROP CONSTRAINT ' + QUOTENAME(f.name) + ';' + CHAR(13)
             FROM sys.foreign_keys AS f
             INNER JOIN sys.tables AS t ON f.parent_object_id = t.object_id
             INNER JOIN sys.schemas AS s ON t.schema_id = s.schema_id
             WHERE s.name = @SchemaName
             
             EXECUTE sp_executesql @SQL;
         END
         GO

         """;

    public static string DropAllTablesAndSchema(string schemaName) =>
        $"""
         IF EXISTS (SELECT * FROM sys.schemas WHERE name = '{schemaName}')
         BEGIN
             DECLARE @SchemaName NVARCHAR(128)
             SET @SchemaName = '{schemaName}'
             
             DECLARE @SQL NVARCHAR(MAX) = ''
             
             -- Drop tables
             SELECT @SQL = @SQL + 'DROP TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + ';' + CHAR(13)
             FROM sys.tables AS t
             INNER JOIN sys.schemas AS s ON t.schema_id = s.schema_id
             WHERE s.name = @SchemaName
             
             -- Drop schema
             SELECT @SQL = @SQL + 'DROP SCHEMA ' + QUOTENAME(@SchemaName) + ';' + CHAR(13)
             
             EXECUTE sp_executesql @SQL;
         END
         GO

         """;
}