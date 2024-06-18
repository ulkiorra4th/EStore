using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using EStore.Application.Interfaces.Repositories;
using EStore.Domain.Models;
using EStore.Infrastructure.Abstractions.Mappers.Abstractions;
using EStore.Persistence.DbConnection;
using EStore.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EStore.Persistence.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper<Product, ProductEntity> _productMapper;
    private readonly ICollectionMapper<Product, ProductEntity> _productCollectionMapper;

    public ProductRepository(AppDbContext context, ICollectionMapper<Product, ProductEntity> productCollectionMapper, 
        IMapper<Product, ProductEntity> productMapper)
    {
        _context = context;
        _productCollectionMapper = productCollectionMapper;
        _productMapper = productMapper;
    }

    [SuppressMessage("ReSharper.DPA", "DPA0000: DPA issues")]
    public async Task<List<Product>> GetAll()
    {
        return _productCollectionMapper
            .MapFrom(
                await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .ToListAsync()
            )
            .Value.ToList();
    }
    
    public async Task<Result<Product>> GetProduct(Guid id)
    {
        var productEntity = await _context.Products
            .AsNoTracking()
            .Include(p=> p.Category)
            .Include(p => p.Seller)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        return _productMapper.MapFrom(productEntity);
    }
}