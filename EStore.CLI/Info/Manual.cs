using Newtonsoft.Json;

namespace EStoreCLI.Info;

internal sealed class Manual
{
    private readonly List<Command> _supportedCommands;
    
    public Manual(string manual) =>
        _supportedCommands = JsonConvert.DeserializeObject<List<Command>>(manual) ?? new List<Command>();
    
    public IReadOnlyCollection<Command> GetSupportedCommands() => _supportedCommands;
}