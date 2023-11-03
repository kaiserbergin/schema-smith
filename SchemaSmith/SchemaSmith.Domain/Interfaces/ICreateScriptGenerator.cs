namespace SchemaSmith.Domain.Interfaces;

public interface ICreateScriptGenerator<in TSchema> where TSchema : class
{
    public string Generate(TSchema schema);
}