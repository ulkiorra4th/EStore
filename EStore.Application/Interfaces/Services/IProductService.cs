using CSharpFunctionalExtensions;
using EStore.Domain.Models;

namespace EStore.Application.Interfaces.Services;

public interface IProductService
{
    Task<List<Product>> GetAll();
    Task<Result<Product>> GetProduct(Guid id);
}