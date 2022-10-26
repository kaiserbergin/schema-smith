namespace CodeGenerators.dotnet;

partial class GraphrGenerator
{
    private string _message;

    public GraphrGenerator(string? message)
    {
        _message = message ?? "Hello World!";
    }
}