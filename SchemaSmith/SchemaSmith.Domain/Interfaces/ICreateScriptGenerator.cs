namespace SchemaSmith.Domain.Interfaces;

public interface ICreateScriptGenerator<TSchema> where TSchema : class
{
    public string GenerateCreateScript(TSchema schema);
}