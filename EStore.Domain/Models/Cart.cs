using CSharpFunctionalExtensions;

namespace EStore.Domain.Models;

public sealed class Cart
{
    private readonly List<Product> _products;

    public Guid Id { get; }
    public IReadOnlyCollection<Product> Products => _products;
    
    private Cart(Guid id, List<Product> products)
    {
        Id = id;
        _products = products;
    }

    public static Result<Cart> CreateEmpty(Guid id)
    {
        return String.IsNullOrEmpty(id.ToString()) 
            ? Result.Failure<Cart>($"{nameof(id)} is empty") 
            : Result.Success(new Cart(id, new List<Product>()));
    }
    
    public static Result<Cart> Create(Guid id, List<Product> products)
    {
        return String.IsNullOrEmpty(id.ToString()) 
            ? Result.Failure<Cart>($"{nameof(id)} is empty") 
            : Result.Success(new Cart(id, products));
    }
    
    public void AddProduct(Product product) => _products.Add(product);
}