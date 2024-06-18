using CSharpFunctionalExtensions;
using EStore.Domain.Extensions;

namespace EStore.Domain.Models;

public sealed class Category
{
    public uint Id { get; }
    public string Name { get; }
    public DateTime CreationDate { get; }

    private Category(uint id, string name, DateTime creationDate)
    {
        Id = id;
        Name = name;
        CreationDate = creationDate;
    }

    public static Result<Category> Create(uint id, string name, DateTime creationDate)
    {
        if (String.IsNullOrEmpty(name))
            return Result.Failure<Category>($"{nameof(name)} is empty");
        if (creationDate.SetKindUtc() > DateTime.UtcNow)
            return Result.Failure<Category>($"incorrect {nameof(creationDate)}");
        
        return Result.Success(new Category(id, name, creationDate));
    }
}