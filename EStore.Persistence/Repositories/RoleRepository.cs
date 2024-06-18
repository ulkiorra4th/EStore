using CSharpFunctionalExtensions;
using EStore.Application.Interfaces.Repositories;
using EStore.Domain.Models;
using EStore.Infrastructure.Abstractions.Mappers.Abstractions;
using EStore.Persistence.DbConnection;
using EStore.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EStore.Persistence.Repositories;

internal sealed class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper<Role, RoleEntity> _roleMapper;

    public RoleRepository(AppDbContext context, IMapper<Role, RoleEntity> roleMapper)
    {
        _context = context;
        _roleMapper = roleMapper;
    }
    
    public async Task<Result<Role>> GetRoleByUserId(Guid userId)
    {
        var roleEntity = await _context.Users
            .AsNoTracking()
            .Where(u => u.UserId == userId)
            .Select(u => u.UserRole)
            .FirstOrDefaultAsync();

        return _roleMapper.MapFrom(roleEntity);
    }
}