using System.Text;
using SchemaSmith.Neo4j.Domain.Dto;
using Index = SchemaSmith.Neo4j.Domain.Dto.Index;

namespace SchemaSmith.Neo4j.Core.ScriptGeneration.ExtensionMethods;

public static class IndexExtensions
{
    private const string BTREE_INDEX = "BTREE";
    private const string TEXT = "TEXT";
    private const string POINT = "POINT";
    private const string RANGE = "RANGE";
    
    private static readonly Dictionary<IndexType, string> _indexTypeToQueryFragment = new ()
    {
        { IndexType.BTree, BTREE_INDEX },
        { IndexType.Text, TEXT},
        { IndexType.Point, POINT }, 
        { IndexType.Range, RANGE}
    };

    public static string GenerateCypher(this Index index)
    {
        var sb = new StringBuilder();

        sb.Append($"CREATE {_indexTypeToQueryFragment[index.Type]} INDEX {index.Name} IF NOT EXISTS");

        switch (index.Entity.Type)
        {
            case EntityType.Node:
                sb.Append($"\nFOR (i:{index.Entity.Id}) ON");
                break;
            case EntityType.Relationship:
                sb.Append($"\nFOR ()-[i:{index.Entity.Id}]-() ON");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        sb.Append("\n(");

        var propCount = 0;

        foreach (var property in index.Entity.Properties)
        {
            sb.Append(propCount == 0 ? "\n" : ",\n");
            sb.Append($"  i.{property}");

            propCount++;
        }

        sb.Append("\n);");

        return sb.ToString();
    }
}