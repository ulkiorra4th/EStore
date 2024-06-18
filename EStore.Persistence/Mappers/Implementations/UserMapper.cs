using CSharpFunctionalExtensions;
using EStore.Domain.Models;
using EStore.Infrastructure.Abstractions.Mappers.Abstractions;
using EStore.Persistence.Entities;

namespace EStore.Persistence.Mappers.Implementations;

internal sealed class UserMapper : IMapper<User, UserEntity>
{
    private readonly IMapper<Role, RoleEntity> _roleMapper;
    private readonly IMapper<Cart, CartEntity> _cartMapper;
    
    public UserMapper(IMapper<Role, RoleEntity> roleMapper, IMapper<Cart, CartEntity> cartMapper)
    {
        _roleMapper = roleMapper;
        _cartMapper = cartMapper;
    }

    public Result<User> MapFrom(UserEntity? obj)
    {
        if (obj is null) return Result.Failure<User>("argument is null");

        Role? role = null;
        Cart? cart = null;

        if (obj.UserRole is not null)
        {
            var roleResult = _roleMapper.MapFrom(obj.UserRole);
            if (roleResult.IsFailure) return Result.Failure<User>(roleResult.Error);
            role = roleResult.Value;
        }

        if (obj.UserCart is not null)
        {
            var cartResult = _cartMapper.MapFrom(obj.UserCart);
            if (cartResult.IsFailure) return Result.Failure<User>(cartResult.Error);
            cart = cartResult.Value;
        }

        return User.Create(obj.UserId, obj.UserName, obj.PasswordHash, 
            role, cart, obj.BirthDate, obj.CreationDate);
    }

    public Result<UserEntity> MapFrom(User? obj)
    {
        if (obj is null) return Result.Failure<UserEntity>("argument is null");

        var roleResult = _roleMapper.MapFrom(obj.UserRole);
        if (roleResult.IsFailure) return Result.Failure<UserEntity>(roleResult.Error);
        
        var cartResult = _cartMapper.MapFrom(obj.UserCart);
        if (cartResult.IsFailure) return Result.Failure<UserEntity>(cartResult.Error);

        return Result.Success(new UserEntity
        {
            UserId = obj.UserId,
            UserName = obj.UserName,
            BirthDate = obj.BirthDate,
            CreationDate = obj.CreationDate,
            PasswordHash = obj.PasswordHash,
            UserRoleId = roleResult.Value.Id,
        });
    }
}