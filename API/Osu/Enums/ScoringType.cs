namespace API.Osu.Enums;

/// <summary>
/// Represents the scoring method (win condition) used for a game
/// </summary>
public enum ScoringType
{
    /// <summary>
    /// Scoring based on Score v1
    /// </summary>
    Score = 0,

    /// <summary>
    /// Scoring based on accuracy
    /// </summary>
    Accuracy = 1,

    /// <summary>
    /// Scoring based on combo
    /// </summary>
    Combo = 2,

    /// <summary>
    /// Scoring based on Score v2
    /// </summary>
    ScoreV2 = 3
}
