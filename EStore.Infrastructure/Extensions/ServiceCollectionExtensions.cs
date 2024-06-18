using EStore.Infrastructure.Tools;
using EStore.Infrastructure.Tools.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EStore.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureTools(this IServiceCollection services)
    {
        services.AddSingleton<IDateParser, DateParser>();
        return services;
    }
}