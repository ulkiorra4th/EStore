using CSharpFunctionalExtensions;
using EStore.Application.Interfaces.Repositories;
using EStore.Application.Interfaces.Services;
using EStore.Domain.Models;

namespace EStore.Application.Services;

internal sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> GetAll()
    {
        return await _productRepository.GetAll();
    }

    public async Task<Result<Product>> GetProduct(Guid id)
    {
        return await _productRepository.GetProduct(id);
    }
}