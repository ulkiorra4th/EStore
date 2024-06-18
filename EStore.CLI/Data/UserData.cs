using EStore.Domain.Extensions;
using EStore.Domain.Models;

namespace EStoreCLI.Data;

internal static class UserData
{
    public static Guid UserId { get; private set; }
    public static string UserName { get; private set; } = null!;
    public static Role UserRole { get; private set; } = null!;
    public static DateTime BirthDate { get; private set; }
    public static DateTime CreationDate { get; private set; }

    public static int Age => BirthDate.CountAge();

    public static void Load(Guid userId, string userName, Role userRole, DateTime birthDate, DateTime creationDate)
    {
        UserId = userId;
        UserName = userName;
        UserRole = userRole;
        BirthDate = birthDate;
        CreationDate = creationDate;
    }
}