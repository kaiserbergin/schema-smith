
using Graphr.Neo4j.Configuration;
using SchemaSmith.Domain;
using Index = SchemaSmith.Domain.Index;

namespace SchemaSmith.DbIntrospection;

public interface INeoSchemaRepository
{
    List<Constraint> GetConstraints();
    List<Index> GetIndexes();
    (List<Node> Nodes, List<Relationship> Relationships) GetDatabaseEntities();
    ServerSchema GetServerSchema();
}