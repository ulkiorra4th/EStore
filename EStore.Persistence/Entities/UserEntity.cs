using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStore.Persistence.Entities;

public sealed class UserEntity
{
    [Key] public Guid UserId { get; set; }
    [Required] public string? UserName { get; set; }
    [Required] public string? PasswordHash { get; set; }
    [ForeignKey("UserRole")] public uint UserRoleId { get; set; }
    [Required] public RoleEntity? UserRole { get; set; }
    [Required] public CartEntity? UserCart { get; set; }
    [Required] public DateTime BirthDate { get; set; }
    [Required] public DateTime CreationDate { get; set; }
    public List<ProductEntity>? ProductsOnSale { get; set; }
}