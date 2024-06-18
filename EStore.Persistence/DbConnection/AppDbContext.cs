using EStore.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EStore.Persistence.DbConnection;

internal sealed class AppDbContext : DbContext
{
    public DbSet<CartEntity> Carts { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}