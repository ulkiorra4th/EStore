using EStore.Application.Interfaces.Repositories;
using EStore.Domain.Models;
using EStore.Infrastructure.Abstractions.Mappers.Abstractions;
using EStore.Persistence.DbConnection;
using EStore.Persistence.Entities;
using EStore.Persistence.Mappers.Implementations;
using EStore.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EStore.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContextAndRepositories(this IServiceCollection services, 
        string connectionString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IProductRepository, ProductRepository>();
        services.AddSingleton<IRoleRepository, RoleRepository>();

        return services;
    }

    public static IServiceCollection AddPersistenceMappers(this IServiceCollection services)
    {
        services.AddSingleton<IMapper<User, UserEntity>, UserMapper>();
        services.AddSingleton<IMapper<Role, RoleEntity>, RoleMapper>();
        services.AddSingleton<IMapper<Product, ProductEntity>, ProductMapper>();
        services.AddSingleton<ICollectionMapper<Product, ProductEntity>, ProductMapper>();
        services.AddSingleton<IMapper<Category, CategoryEntity>, CategoryMapper>();
        services.AddSingleton<IMapper<Cart, CartEntity>, CartMapper>();

        return services;
    }
}