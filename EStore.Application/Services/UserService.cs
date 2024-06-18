using CSharpFunctionalExtensions;
using EStore.Application.Interfaces.Repositories;
using EStore.Application.Interfaces.Services;
using EStore.Domain.Constants;
using EStore.Domain.Extensions;
using EStore.Domain.Models;

namespace EStore.Application.Services;

internal sealed class UserService : IUserService
{
    private const int MinUserAge = 16;
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<Guid>> Register(string userName, string password, DateTime birthDate)
    {
        if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
            return Result.Failure<Guid>($"{nameof(userName)} or {nameof(password)} is empty");

        if (birthDate.CountAge() < MinUserAge)
            return Result.Failure<Guid>($"user's age should be ge {MinUserAge}");
        
        if (await Exists(userName))
            return Result.Failure<Guid>($"user with {nameof(userName)} {userName} already exists");
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        
        var userResult = User.Create(Guid.NewGuid(), userName, hashedPassword, 
            Role.CreateEmpty((int)RoleType.Customer).Value, Cart.CreateEmpty(Guid.NewGuid()).Value, birthDate, 
            DateTime.UtcNow);

        return userResult.IsFailure
            ? Result.Failure<Guid>(userResult.Error)
            : await _userRepository.AddUser(userResult.Value);
    }

    public async Task<Result<User>> Login(string userName, string password)
    {
        if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
            return Result.Failure<User>($"{nameof(userName)} or {nameof(password)} is empty");

        var userResult = await _userRepository.GetUserByUserName(userName);
        
        if (userResult.IsFailure) 
            return Result.Failure<User>($"user with {nameof(userName)} {userName} doesn't exist");

        return !BCrypt.Net.BCrypt.Verify(password, userResult.Value.PasswordHash) 
            ? Result.Failure<User>($"{nameof(userName)} or {nameof(password)} is incorrect") 
            : Result.Success(userResult.Value);
    }

    public async Task<Result<User>> GetById(Guid id)
    {
        return await _userRepository.GetUserById(id);
    }

    public async Task<Result<User>> GetWithCartById(Guid id)
    {
        return await _userRepository.GetUserWithCartById(id);
    }

    public async Task<Result<User>> GetByUserName(string userName)
    {
        return await _userRepository.GetUserByUserName(userName);
    }

    public async Task<Result<User>> GetUserWithRoleById(Guid id)
    {
        return await _userRepository.GetUserWithRoleById(id);
    }
    
    public async Task<Result> AddProductToCart(Guid userId, Guid productId)
    {
        return await _userRepository.AddProductToCart(productId, userId);
    }

    public async Task<Result> DeleteProductFromCart(Guid userId, Guid productId)
    {
        return await _userRepository.DeleteProductFromCart(productId, userId);
    }

    public async Task<bool> Exists(string userName)
    {
        var userResult = await GetByUserName(userName);
        return userResult.IsSuccess;
    }
}