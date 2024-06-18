using EStoreCLI.Data;
using EStoreCLI.Info;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace EStoreCLI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, string configFilePath)
    {
        services.AddSingleton<Configuration>(o => 
            new Configuration(JObject.Parse(File.ReadAllText(configFilePath))));

        return services;
    }
}