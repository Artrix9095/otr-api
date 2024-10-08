using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Database.Entities;

/// <summary>
/// Describes the performance of a <see cref="Entities.Player"/> over all <see cref="Game"/>s in a <see cref="Entities.Match"/>
/// </summary>
[Table("player_match_stats")]
[SuppressMessage("ReSharper", "EntityFramework.ModelValidation.CircularDependency")]
public class PlayerMatchStats : EntityBase
{
    /// <summary>
    /// Match cost
    /// </summary>
    [Column("match_cost")]
    public double MatchCost { get; init; }

    /// <summary>
    /// Average score
    /// </summary>
    [Column("average_score")]
    public double AverageScore { get; init; }

    /// <summary>
    /// Average placement based on score
    /// </summary>
    [Column("average_placement")]
    public double AveragePlacement { get; init; }

    /// <summary>
    /// Average miss count
    /// </summary>
    [Column("average_misses")]
    public double AverageMisses { get; init; }

    /// <summary>
    /// Average accuracy
    /// </summary>
    [Column("average_accuracy")]
    public double AverageAccuracy { get; init; }

    /// <summary>
    /// Total number of games played
    /// </summary>
    [Column("games_played")]
    public int GamesPlayed { get; init; }

    /// <summary>
    /// Total number of games won
    /// </summary>
    [Column("games_won")]
    public int GamesWon { get; init; }

    /// <summary>
    /// Total number of games lost
    /// </summary>
    [Column("games_lost")]
    public int GamesLost { get; init; }

    /// <summary>
    /// Denotes if the <see cref="Player"/> won
    /// </summary>
    [Column("won")]
    public bool Won { get; init; }

    /// <summary>
    /// List of ids of the <see cref="Player"/>'s teammates
    /// </summary>
    [Column("teammate_ids")]
    public int[] TeammateIds { get; init; } = [];

    /// <summary>
    /// List of ids of the <see cref="Player"/>'s opponents
    /// </summary>
    [Column("opponent_ids")]
    public int[] OpponentIds { get; init; } = [];

    /// <summary>
    /// Id of the <see cref="Entities.Player"/> the <see cref="PlayerMatchStats"/> was generated for
    /// </summary>
    [Column("player_id")]
    public int PlayerId { get; init; }

    /// <summary>
    /// The <see cref="Entities.Player"/> the <see cref="PlayerMatchStats"/> was generated for
    /// </summary>
    public Player Player { get; init; } = null!;

    /// <summary>
    /// Id of the <see cref="Entities.Match"/> the <see cref="PlayerMatchStats"/> was generated for
    /// </summary>
    [Column("match_id")]
    public int MatchId { get; init; }

    /// <summary>
    /// The <see cref="Entities.Match"/> the <see cref="PlayerMatchStats"/> was generated for
    /// </summary>
    public Match Match { get; init; } = null!;
}
