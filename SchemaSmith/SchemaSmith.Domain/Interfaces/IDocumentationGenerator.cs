namespace SchemaSmith.Domain.Interfaces;

public interface IDocumentationGenerator<in TSchema> where TSchema : class
{
    public string Generate(TSchema schema);
}