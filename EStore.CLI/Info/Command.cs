namespace EStoreCLI.Info;

internal sealed class Command
{
    public required string Name { get; init; } 
    public required string Syntax { get; init; }
    public required IReadOnlyDictionary<string, string> ArgumentsAndDescriptions { get; init; } =
        new Dictionary<string, string>();
    public required string Description { get; init; }
}