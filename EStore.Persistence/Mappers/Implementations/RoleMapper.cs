using CSharpFunctionalExtensions;
using EStore.Domain.Models;
using EStore.Infrastructure.Abstractions.Mappers.Abstractions;
using EStore.Persistence.Entities;

namespace EStore.Persistence.Mappers.Implementations;

internal sealed class RoleMapper : IMapper<Role, RoleEntity>
{
    public Result<Role> MapFrom(RoleEntity? obj)
    {
        return obj is null 
            ? Result.Failure<Role>("argument is null")
            : Role.Create(obj.Id, obj.Name, obj.CanSell, obj.CanEdit, obj.CanBuy);
    }

    public Result<RoleEntity> MapFrom(Role? obj)
    {
        return obj is null 
            ? Result.Failure<RoleEntity>("argument is null")
            : Result.Success(new RoleEntity
            {
                Id = obj.Id,
                Name = obj.Name,
                CanBuy = obj.CanBuy,
                CanEdit = obj.CanEdit,
                CanSell = obj.CanSell
            });
    }
}