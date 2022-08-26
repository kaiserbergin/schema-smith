using System.Text;
using SchemaSmith.Domain;

namespace SchemaSmith.CypherGenerator;

internal static class ConstraintExtensions
{
    private static readonly Dictionary<ConstraintType, string> SuffixDict = new ()
    {
        {
            ConstraintType.NodeKey, "IS NODE KEY"
        },
        {
            ConstraintType.Unique ,"IS UNIQUE"
        },
        {
            ConstraintType.Existence, "IS NOT NULL"
        }
    };

    internal static string GenerateCypher(this Constraint constraint) =>
        constraint.Entity.Type switch
        {
            EntityType.Node => CreateNodeConstraintCypher(constraint),
            EntityType.Relationship => CreateRelationshipConstraintCypher(constraint),
            _ => throw new ArgumentOutOfRangeException()
        };

    private static string CreateRelationshipConstraintCypher(Constraint constraint)
    {
        var sb = new StringBuilder();

        sb.Append($"CREATE CONSTRAINT {constraint.Name} IF NOT EXISTS");
        sb.Append($"\nFOR ()-[r:{constraint.Entity.Id}]-()");
        sb.Append($"\nREQUIRE r.{constraint.Entity.Properties.Single()} IS NOT NULL;");

        return sb.ToString();
    }

    private static string CreateNodeConstraintCypher(Constraint constraint)
    {
        var sb = new StringBuilder();

        sb.Append($"CREATE CONSTRAINT {constraint.Name} IF NOT EXISTS");
        sb.Append($"\nFOR (n:{constraint.Entity.Id}) ");
        CreateRequireNormalPropertiesFragment(constraint, sb);
        sb.Append($"\n{SuffixDict[constraint.Type]};");

        return sb.ToString();
    }

    private static void CreateRequireNormalPropertiesFragment(Constraint constraint, StringBuilder sb)
    {
        var propCount = 0;
        
        sb.Append("\n REQUIRE (");

        foreach (var property in constraint.Entity.Properties)
        {
            sb.Append(propCount == 0 ? "\n " : ",\n ");
            sb.Append($"n.{property}");

            propCount++;
        }

        sb.Append("\n)");
    }
}