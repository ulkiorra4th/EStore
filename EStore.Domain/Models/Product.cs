using CSharpFunctionalExtensions;
using EStore.Domain.Extensions;

namespace EStore.Domain.Models;

public sealed class Product
{
    public Guid Id { get;}
    public string Name { get; }
    public string? Description { get; }
    public User? Seller { get; }
    public decimal Cost { get; private set; }
    public Category? Category { get; }
    public DateTime CreationDate { get; }

    private Product(Guid id, string name, string? description, User? seller, decimal cost, Category? category, 
        DateTime creationDate)
    {
        Id = id;
        Name = name;
        Description = description;
        Seller = seller;
        Cost = cost;
        Category = category;
        CreationDate = creationDate;
    }

    public static Result<Product> Create(Guid id, string name, string? description, User? seller, decimal cost,
        Category? category, DateTime creationDate)
    {
        if (String.IsNullOrEmpty(name))
            return Result.Failure<Product>($"{nameof(name)} is empty");
        
        if (String.IsNullOrEmpty(id.ToString()))
            return Result.Failure<Product>($"{nameof(id)} is empty");
        
        if (decimal.IsNegative(cost))
            return Result.Failure<Product>($"{nameof(cost)} is negative");
        
        if (creationDate.SetKindUtc() > DateTime.UtcNow)
            return Result.Failure<Product>($"incorrect {nameof(creationDate)}");

        return Result.Success(new Product(id, name, description, seller, cost, category, creationDate));
    }

    public Result SetCost(decimal newCost) 
    {
        if (decimal.IsNegative(newCost))
            return Result.Failure($"{newCost} is negative");

        Cost = newCost;
        
        return Result.Success();
    }
}