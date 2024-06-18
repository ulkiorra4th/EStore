using System.ComponentModel.DataAnnotations;

namespace EStore.Persistence.Entities;

public sealed class CategoryEntity
{
    [Key] public uint Id { get; set; }
    [Required] public string? Name { get; set; }
    [Required] public DateTime CreationDate { get; set; }
    public List<ProductEntity>? Products { get; set; }
}
