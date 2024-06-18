using CSharpFunctionalExtensions;

namespace EStore.Domain.Models;

public sealed class Role
{
    public uint Id { get; }
    public string Name { get; } = null!;
    public bool CanBuy { get; }
    public bool CanSell { get; }
    public bool CanEdit { get; }

    private Role(uint id)
    {
        Id = id;
    }
    
    private Role(bool canSell, bool canEdit, bool canBuy, string name, uint id) : this(id)
    {
        CanSell = canSell;
        CanEdit = canEdit;
        CanBuy = canBuy;
        Name = name;
    }

    public static Result<Role> Create(uint id, string name, bool canSell, bool canEdit, bool canBuy)
    {
        return String.IsNullOrEmpty(name) 
            ? Result.Failure<Role>($"{nameof(name)} is empty") 
            : Result.Success(new Role(canSell, canEdit, canBuy, name, id));
    }

    public static Result<Role> CreateEmpty(uint id)
    {
        return Result.Success(new Role(id));
    }
}