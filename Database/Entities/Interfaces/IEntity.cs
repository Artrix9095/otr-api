﻿namespace Database.Entities.Interfaces;

/// <summary>
/// Interfaces an entity
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Primary key of the entity
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Date the entity was created
    /// </summary>
    public DateTime Created { get; }
}
