using System.Text;
using SchemaSmith.Domain;

namespace SchemaSmith.CypherGenerator;

public static class ConstraintExtensions
{
    private static readonly Dictionary<string, string> SuffixDict = new Dictionary<string, string>
    {
        {
            "node-key", "IS NODE KEY"
        },
        {
            "unique", "IS UNIQUE"
        },
        {
            "existence", "IS NOT NULL"
        }
    };

    public static string GenerateCypher(this Constraint constraint) =>
        constraint.Entity.Type switch
        {
            EntityType.Node => "asdf",
            EntityType.Relationship => "asdf"
        };

    private static string CreateNodeKeyConstraintCypher(Constraint constraint)
    {
        if (constraint.Type == "existence")
            return CreateExistenceConstraintCypher(Constraint constraint);

        var sb = new StringBuilder();

        sb.Append($"CREATE CONSTRAINT {constraint.Name} IF NOT EXISTS");
        sb.Append($"\nFOR (nc:{constraint.Entity.Id}) ");
        CreateRequireNormalPropertiesFragment(constraint, sb);
        sb.Append($"\n{SuffixDict[constraint.Type]}");

        return sb.ToString();
    }

    private static void CreateRequireNormalPropertiesFragment(Constraint constraint, StringBuilder sb)
    {
        var propCount = 0;
        
        sb.Append("\n Require (");

        foreach (var property in constraint.Entity.Properties)
        {
            sb.Append(propCount == 0 ? "\n " : ",\n ");
            sb.Append($"cn.{property}");
        }

        sb.Append('\n');
    }
}