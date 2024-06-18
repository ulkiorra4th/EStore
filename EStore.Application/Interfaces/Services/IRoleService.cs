using CSharpFunctionalExtensions;
using EStore.Domain.Models;

namespace EStore.Application.Interfaces.Services;

public interface IRoleService
{
    Task<Result<Role>> GetRoleByUserId(Guid userId);
}