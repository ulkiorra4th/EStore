using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStore.Persistence.Entities;

public sealed class CartEntity
{
    [Key] public Guid Id { get; set; }
    [ForeignKey("User")] public Guid UserId { get; set; }
    [Required] public UserEntity? User { get; set; }
    public List<ProductEntity>? Products { get; set; }
}