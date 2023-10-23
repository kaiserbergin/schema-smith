using SchemaSmith.Domain.Interfaces;
using SchemaSmith.Neo4j.Domain.Dto;

namespace SchemaSmith.Neo4j.Core.ScriptGeneration;

public class CreateScriptGenerator : ICreateScriptGenerator<ServerSchema>
{
    public string GenerateCreateScript(ServerSchema schema)
    {
        
    }
}