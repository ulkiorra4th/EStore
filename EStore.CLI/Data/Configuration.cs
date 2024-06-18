using Newtonsoft.Json.Linq;

namespace EStoreCLI.Data;

internal sealed class Configuration
{
    private readonly JObject _config;

    public Configuration(JObject config)
    {
        _config = config;
    }

    public T? GetValue<T>(string key) => _config.Value<T>(key);

    public string? GetConnectionString(string key) => _config.Value<JObject>("ConnectionStrings")?.Value<string>(key);
}