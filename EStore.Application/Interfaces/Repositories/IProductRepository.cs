using CSharpFunctionalExtensions;
using EStore.Domain.Models;

namespace EStore.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAll();
    Task<Result<Product>> GetProduct(Guid id);
}