using System.Text;
using SchemaSmith.Domain.Interfaces;
using SchemaSmith.Neo4j.Core.ScriptGeneration.ExtensionMethods;
using SchemaSmith.Neo4j.Domain.Dto;

namespace SchemaSmith.Neo4j.Core.ScriptGeneration;

public class CreateScriptGenerator : ICreateScriptGenerator<GraphSchema>
{
    public string Generate(GraphSchema graphSchema)
    {
        var sb = new StringBuilder();
        
        graphSchema.GenerateCypherStatements()
            .ToList()
            .ForEach(graphString => sb.AppendLine(graphString));
        
        return sb.ToString();
    }
}