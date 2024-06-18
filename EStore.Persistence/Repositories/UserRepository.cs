using CSharpFunctionalExtensions;
using EStore.Application.Interfaces.Repositories;
using EStore.Domain.Models;
using EStore.Infrastructure.Abstractions.Mappers.Abstractions;
using EStore.Persistence.DbConnection;
using EStore.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EStore.Persistence.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper<User, UserEntity> _userMapper;

    public UserRepository(AppDbContext context, IMapper<User, UserEntity> userMapper)
    {
        _context = context;
        _userMapper = userMapper;
    }

    public async Task<Result<User>> GetUserWithCartById(Guid id)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .Include(u => u.UserCart)
            .Include(u => u.UserCart!.Products)
            .FirstOrDefaultAsync(u => u.UserId == id);
        
        return _userMapper.MapFrom(userEntity);
    }

    public async Task<Result<User>> GetUserById(Guid id)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == id);

        return _userMapper.MapFrom(userEntity);
    }
    
    public async Task<Result<User>> GetUserByUserName(string userName)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName == userName);

        return _userMapper.MapFrom(userEntity);
    }

    public async Task<Result<User>> GetUserWithRoleById(Guid id)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .Include(u => u.UserRole)
            .FirstOrDefaultAsync(u => u.UserId == id);

        return _userMapper.MapFrom(userEntity);
    }

    public async Task<Result<Guid>> AddUser(User user)
    {
        var entityResult = _userMapper.MapFrom(user);
        if (entityResult.IsFailure) return Result.Failure<Guid>("invalid argument");

        _context.Users.Add(entityResult.Value);
        await _context.SaveChangesAsync();
        return Result.Success(entityResult.Value.UserId);
    }

    public async Task<Result> AddProductToCart(Guid productId, Guid userId)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == productId);
        if (product is null) return Result.Failure($"product with id {productId} doesn't exist");
            
        var user = await _context.Users
            .Include(u => u.UserCart)
            .FirstOrDefaultAsync(u => u.UserId == userId);
            
        if (user is null) return Result.Failure($"user with id {userId} doesn't exist");

        user.UserCart!.Products ??= new List<ProductEntity>(); 
        user.UserCart.Products.Add(product);
            
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteProductFromCart(Guid productId, Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.UserCart)
            .Include(u => u.UserCart!.Products)
            .FirstOrDefaultAsync(u => u.UserId == userId);
        
        if (user is null) return Result.Failure($"user with id {userId} doesn't exist");
        if (user.UserCart!.Products is null) return Result.Failure("cart is empty");

        var product = user.UserCart.Products!.FirstOrDefault(p => p.Id == productId);
        if (product is null) return Result.Failure($"product with id {productId} doesn't exist in cart");

        user.UserCart.Products!.Remove(product);
        
        await _context.SaveChangesAsync();
        return Result.Success();
    }
}