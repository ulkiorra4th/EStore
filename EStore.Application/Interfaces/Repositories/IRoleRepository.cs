using CSharpFunctionalExtensions;
using EStore.Domain.Models;

namespace EStore.Application.Interfaces.Repositories;

public interface IRoleRepository
{ 
    Task<Result<Role>> GetRoleByUserId(Guid userId);
}