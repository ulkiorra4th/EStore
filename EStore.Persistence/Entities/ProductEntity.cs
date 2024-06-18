using System.ComponentModel.DataAnnotations;

namespace EStore.Persistence.Entities;

public sealed class ProductEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public string? Name { get; set; }
    public string? Description { get; set; }
    [Required] public UserEntity? Seller { get; set; }
    [Required] public decimal Cost { get; set; }
    [Required] public CategoryEntity? Category { get; set; }
    [Required] public DateTime CreationDate { get; set; }
    public List<CartEntity>? Carts { get; set; }
}