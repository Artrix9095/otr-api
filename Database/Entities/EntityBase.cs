using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Entities.Interfaces;

namespace Database.Entities;

/// <summary>
/// Base entity
/// </summary>
public abstract class EntityBase : IEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Column("created")]
    public DateTime Created { get; init; }
}
