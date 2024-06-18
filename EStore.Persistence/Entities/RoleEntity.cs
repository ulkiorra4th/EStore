using System.ComponentModel.DataAnnotations;

namespace EStore.Persistence.Entities;

public sealed class RoleEntity
{
    [Key] public uint Id { get; set; }
    [Required] public string? Name { get; set; }
    [Required] public bool CanBuy { get; set; }
    [Required] public bool CanSell { get; set; }
    [Required] public bool CanEdit { get; set; }
    public List<UserEntity>? Users { get; set; }
}