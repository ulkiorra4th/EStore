using CSharpFunctionalExtensions;
using EStore.Domain.Models;
using EStore.Infrastructure.Abstractions.Mappers.Abstractions;
using EStore.Persistence.Entities;

namespace EStore.Persistence.Mappers.Implementations;

internal sealed class CartMapper : IMapper<Cart, CartEntity>
{
    private readonly ICollectionMapper<Product, ProductEntity> _productCollectionMapper;

    public CartMapper(ICollectionMapper<Product, ProductEntity> productCollectionMapper)
    {
        _productCollectionMapper = productCollectionMapper;
    }

    public Result<Cart> MapFrom(CartEntity? obj)
    { 
        if (obj is null) return Result.Failure<Cart>("argument is null");

        var productsResult = _productCollectionMapper.MapFrom(obj.Products!);
        return productsResult.IsFailure 
            ? Result.Failure<Cart>(productsResult.Error) 
            : Cart.Create(obj.Id, productsResult.Value.ToList());
    }

    public Result<CartEntity> MapFrom(Cart? obj)
    {
        if (obj is null) return Result.Failure<CartEntity>("argument is null");

        var productsResult = _productCollectionMapper.MapFrom(obj.Products.ToList());
        return productsResult.IsFailure 
            ? Result.Failure<CartEntity>(productsResult.Error)
            : Result.Success(new CartEntity { Id = obj.Id, Products = productsResult.Value.ToList() });
    }
}