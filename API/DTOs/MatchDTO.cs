using API.Osu.Enums;

namespace API.DTOs;

/// <summary>
/// Represents a played match
/// </summary>
public class MatchDTO
{
    /// <summary>
    /// Id of the match
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// osu! id of the match
    /// </summary>
    public long MatchId { get; set; }

    /// <summary>
    /// Title of the lobby
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Ruleset of the match
    /// </summary>
    public Ruleset Ruleset { get; set; }

    /// <summary>
    /// Start time of the match
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// End time of the match
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Current verification status of the match
    /// </summary>
    public int? VerificationStatus { get; set; }

    /// <summary>
    /// List of games played during the match
    /// </summary>
    public ICollection<GameDTO> Games { get; set; } = new List<GameDTO>();
}
