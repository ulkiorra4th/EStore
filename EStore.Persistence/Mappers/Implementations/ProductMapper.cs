using CSharpFunctionalExtensions;
using EStore.Domain.Models;
using EStore.Infrastructure.Abstractions.Mappers.Abstractions;
using EStore.Persistence.Entities;

namespace EStore.Persistence.Mappers.Implementations;

internal sealed class ProductMapper : IMapper<Product, ProductEntity>, ICollectionMapper<Product, ProductEntity>
{
    private readonly IMapper<Category, CategoryEntity> _categoryMapper;

    public ProductMapper(IMapper<Category, CategoryEntity> categoryMapper)
    {
        _categoryMapper = categoryMapper;
    }

    public Result<Product> MapFrom(ProductEntity? obj)
    {
        if (obj is null) return Result.Failure<Product>("argument is null");
        
        User? seller = null;
        Category? category = null;

        if (obj.Seller is not null)
        {
            var sellerResult = User.CreateEmptySimple(obj.Seller.UserId, obj.Seller.UserName);
            if (sellerResult.IsFailure) return Result.Failure<Product>(sellerResult.Error);
            seller = sellerResult.Value;
        }

        if (obj.Category is not null)
        {
            var categoryResult = _categoryMapper.MapFrom(obj.Category!);
            if (categoryResult.IsFailure) return Result.Failure<Product>(categoryResult.Error);
            category = categoryResult.Value;
        }

        return Product.Create(obj.Id, obj.Name, obj.Description, seller, obj.Cost, category, 
            obj.CreationDate);
    }

    public Result<ProductEntity> MapFrom(Product? obj)
    {
        if (obj is null) return Result.Failure<ProductEntity>("argument is null");
        if (obj.Seller is null) return Result.Failure<ProductEntity>("seller is null");
        
        var categoryResult = _categoryMapper.MapFrom(obj.Category);
        if (categoryResult.IsFailure) return Result.Failure<ProductEntity>(categoryResult.Error);

        return Result.Success(new ProductEntity
        {
            Id = obj.Id,
            Name = obj.Name,
            Description = obj.Description,
            Category = categoryResult.Value,
            Cost = obj.Cost,
            Seller = new UserEntity { UserId = obj.Seller.UserId, UserName = obj.Seller.UserName },
            CreationDate = obj.CreationDate
        });
    }

    public Result<ICollection<Product>> MapFrom(ICollection<ProductEntity>? obj)
    {
        if (obj is null) return Result.Failure<ICollection<Product>>("product collection is null");
        var products = new List<Product>();
        
        foreach (var product in obj)
        {
            var productResult = MapFrom(product);
            if (productResult.IsFailure) return Result.Failure<ICollection<Product>>(productResult.Error);
            products.Add(productResult.Value);
        }

        return products;
    }

    public Result<ICollection<ProductEntity>> MapFrom(ICollection<Product>? obj)
    {
        if (obj is null) return Result.Failure<ICollection<ProductEntity>>("product collection is null");
        var products = new List<ProductEntity>();
        
        foreach (var product in obj)
        {
            var productResult = MapFrom(product);
            if (productResult.IsFailure) return Result.Failure<ICollection<ProductEntity>>(productResult.Error);
            products.Add(productResult.Value);
        }

        return products;
    }
    
}