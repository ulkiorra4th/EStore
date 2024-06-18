using CSharpFunctionalExtensions;
using EStore.Domain.Constants;
using EStore.Domain.Extensions;

namespace EStore.Domain.Models;

public sealed class User
{
    public Guid UserId { get; }
    public string UserName { get; }
    public string PasswordHash { get; }
    public Role? UserRole { get; }
    public Cart? UserCart { get; }
    public DateTime BirthDate { get; }
    public DateTime CreationDate { get; }

    public int Age => BirthDate.CountAge();

    private User(Guid userId, string userName)
    {
        UserId = userId;
        UserName = userName;
    }

    private User(Guid userId, string userName, string passwordHash,
        Role? userRole, Cart? userCart, DateTime birthDate, DateTime creationDate)
    : this(userId, userName)
    {
        UserRole = userRole;
        UserCart = userCart;
        BirthDate = birthDate;
        CreationDate = creationDate;
        PasswordHash = passwordHash;
    }

    public static Result<User> Create(Guid userId, string userName, string passwordHash,
        Role? userRole, Cart? userCart, DateTime birthDate, DateTime creationDate)
    {
        if (String.IsNullOrEmpty(userId.ToString()))
            return Result.Failure<User>($"{nameof(userId)} is empty");
        
        if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(passwordHash))
            return Result.Failure<User>($"{nameof(userName)} or {nameof(passwordHash)} is empty");
        
        if (birthDate.CountAge() < UserConstants.MinAge)
            return Result.Failure<User>($"user's age is lt {UserConstants.MinAge}");
        
        if (creationDate.SetKindUtc() > DateTime.UtcNow)
            return Result.Failure<User>($"incorrect {nameof(creationDate)}");
        
        return Result.Success(new User(userId, userName, passwordHash, userRole, userCart, birthDate, 
            creationDate));
    }

    public static Result<User> CreateEmptySimple(Guid userId, string userName)
    {
        if (String.IsNullOrEmpty(userId.ToString()))
            return Result.Failure<User>($"{nameof(userId)} is empty");
        
        if (String.IsNullOrEmpty(userName))
            return Result.Failure<User>($"{nameof(userName)} is empty");

        return Result.Success(new User(userId, userName));
    }
}