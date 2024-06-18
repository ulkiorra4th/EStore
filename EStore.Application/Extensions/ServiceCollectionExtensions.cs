using EStore.Application.Interfaces.Services;
using EStore.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EStore.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IProductService, ProductService>();
        services.AddSingleton<IRoleService, RoleService>();

        return services;
    }
}