namespace SchemaSmith.Domain.Interfaces;

public interface IIntrospectionRepository<TSchema>
    where TSchema : class
{
    public Task<TSchema> GetServerSchemaAsync();
}