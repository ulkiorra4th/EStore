using CSharpFunctionalExtensions;
using EStore.Domain.Models;

namespace EStore.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Result<User>> GetUserWithCartById(Guid id); 
    Task<Result<User>> GetUserById(Guid id);
    Task<Result<User>> GetUserByUserName(string userName);
    Task<Result<User>> GetUserWithRoleById(Guid id);
    Task<Result<Guid>> AddUser(User entity);
    Task<Result> AddProductToCart(Guid productId, Guid userId);
    Task<Result> DeleteProductFromCart(Guid productId, Guid userId);
}