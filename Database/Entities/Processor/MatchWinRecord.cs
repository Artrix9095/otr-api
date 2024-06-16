using System.ComponentModel.DataAnnotations.Schema;
using Database.Enums;

namespace Database.Entities.Processor;

/// <summary>
/// Describes roster information for both teams in a <see cref="Entities.Match"/>
/// </summary>
/// <remarks>
/// Generated by the <a href="https://github.com/osu-tournament-rating/otr-processor">o!TR Processor</a>
/// </remarks>
[Table("match_win_records")]
public class MatchWinRecord : EntityBase
{
    /// <summary>
    /// List of ids of the players on the winning team
    /// </summary>
    [Column("winner_roster")]
    public int[] WinnerRoster { get; init; } = [];

    /// <summary>
    /// List of ids of the players on the losing team
    /// </summary>
    [Column("loser_roster")]
    public int[] LoserRoster { get; init; } = [];

    /// <summary>
    /// Points scored by the <see cref="WinnerTeam"/>
    /// </summary>
    [Column("winner_points")]
    public int WinnerPoints { get; init; }

    /// <summary>
    /// Points scored by the <see cref="LoserTeam"/>
    /// </summary>
    [Column("loser_points")]
    public int LoserPoints { get; init; }

    /// <summary>
    /// The <see cref="Enums.Team"/> that won the match
    /// </summary>
    [Column("winner_team")]
    public Team? WinnerTeam { get; init; }

    /// <summary>
    /// The <see cref="Enums.Team"/> that lost the match
    /// </summary>
    [Column("loser_team")]
    public Team? LoserTeam { get; init; }

    /// <summary>
    /// The <see cref="OsuMatchType"/> of the match
    /// </summary>
    [Column("match_type")]
    public OsuMatchType? MatchType { get; init; }

    /// <summary>
    /// Id of the <see cref="Entities.Match"/> the <see cref="MatchWinRecord"/> was generated for
    /// </summary>
    [Column("match_id")]
    public int MatchId { get; init; }

    /// <summary>
    /// The <see cref="Entities.Match"/> the <see cref="MatchWinRecord"/> was generated for
    /// </summary>
    public Match Match { get; init; } = null!;
}
