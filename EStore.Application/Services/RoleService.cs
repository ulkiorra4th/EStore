using CSharpFunctionalExtensions;
using EStore.Application.Interfaces.Repositories;
using EStore.Application.Interfaces.Services;
using EStore.Domain.Models;

namespace EStore.Application.Services;

internal sealed class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result<Role>> GetRoleByUserId(Guid userId)
    {
        return await _roleRepository.GetRoleByUserId(userId);
    }
}