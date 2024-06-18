using CSharpFunctionalExtensions;
using EStore.Domain.Models;

namespace EStore.Application.Interfaces.Services;

public interface IUserService
{
    Task<Result<Guid>> Register(string userName, string password, DateTime birthDate); 
    Task<Result<User>> Login(string userName, string password);
    Task<Result<User>> GetById(Guid id);
    Task<Result<User>> GetWithCartById(Guid id);
    Task<Result<User>> GetByUserName(string userName);
    Task<Result<User>> GetUserWithRoleById(Guid id);
    Task<Result> AddProductToCart(Guid userId, Guid productId);
    Task<Result> DeleteProductFromCart(Guid userId, Guid productId);
    Task<bool> Exists(string userName);

}