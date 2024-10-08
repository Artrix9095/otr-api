using System.Diagnostics.CodeAnalysis;
using Database.Entities;
using Database.Enums;
using Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories.Implementations;

[SuppressMessage("Performance", "CA1862:Use the \'StringComparison\' method overloads to perform case-insensitive string comparisons")]
[SuppressMessage("ReSharper", "SpecifyStringComparison")]
public class PlayerMatchStatsRepository(OtrContext context) : IPlayerMatchStatsRepository
{
    public async Task<IEnumerable<PlayerMatchStats>> GetForPlayerAsync(
        int playerId,
        Ruleset ruleset,
        DateTime dateMin,
        DateTime dateMax
    )
    {
        return await context
            .PlayerMatchStats.Where(stats =>
                stats.PlayerId == playerId
                && stats.Match.Tournament.Ruleset == ruleset
                && stats.Match.StartTime >= dateMin
                && stats.Match.StartTime <= dateMax
            )
            .OrderBy(x => x.Match.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<PlayerMatchStats>> TeammateStatsAsync(
        int playerId,
        int teammateId,
        Ruleset ruleset,
        DateTime dateMin,
        DateTime dateMax
    ) =>
        await context
            .PlayerMatchStats.Where(stats =>
                stats.PlayerId == playerId
                && stats.TeammateIds.Contains(teammateId)
                && stats.Match.Tournament.Ruleset == ruleset
                && stats.Match.StartTime >= dateMin
                && stats.Match.StartTime <= dateMax
            )
            .OrderBy(x => x.Match.StartTime)
            .ToListAsync();

    public async Task<IEnumerable<PlayerMatchStats>> OpponentStatsAsync(
        int playerId,
        int opponentId,
        Ruleset mode,
        DateTime dateMin,
        DateTime dateMax
    ) =>
        await context
            .PlayerMatchStats.Where(stats =>
                stats.PlayerId == playerId
                && stats.OpponentIds.Contains(opponentId)
                && stats.Match.Tournament.Ruleset == mode
                && stats.Match.StartTime >= dateMin
                && stats.Match.StartTime <= dateMax
            )
            .OrderBy(x => x.Match.StartTime)
            .ToListAsync();

    public async Task InsertAsync(IEnumerable<PlayerMatchStats> items)
    {
        await context.PlayerMatchStats.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }

    public async Task TruncateAsync() =>
        await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE player_match_stats RESTART IDENTITY;");

    public async Task<int> CountMatchesPlayedAsync(
        int playerId,
        Ruleset ruleset,
        DateTime? dateMin = null,
        DateTime? dateMax = null
    )
    {
        dateMin ??= DateTime.MinValue;
        dateMax ??= DateTime.MaxValue;

        return await context
            .PlayerMatchStats.Where(x =>
                x.PlayerId == playerId
                && x.Match.Tournament.Ruleset == ruleset
                && x.Match.StartTime >= dateMin
                && x.Match.StartTime <= dateMax
            )
            .CountAsync();
    }

    public async Task<int> CountMatchesWonAsync(
        int playerId,
        Ruleset ruleset,
        DateTime? dateMin = null,
        DateTime? dateMax = null
    )
    {
        dateMin ??= DateTime.MinValue;
        dateMax ??= DateTime.MaxValue;

        return await context
            .PlayerMatchStats.Where(x =>
                x.PlayerId == playerId
                && x.Match.Tournament.Ruleset == ruleset
                && x.Won
                && x.Match.StartTime >= dateMin
                && x.Match.StartTime <= dateMax
            )
            .CountAsync();
    }

    public async Task<double> GlobalWinrateAsync(
        int playerId,
        Ruleset ruleset,
        DateTime? dateMin = null,
        DateTime? dateMax = null
    )
    {
        dateMin ??= DateTime.MinValue;
        dateMax ??= DateTime.MaxValue;

        var matchesPlayed = await CountMatchesPlayedAsync(playerId, ruleset, dateMin, dateMax);
        var matchesWon = await CountMatchesWonAsync(playerId, ruleset, dateMin, dateMax);

        if (matchesPlayed == 0)
        {
            return 0;
        }

        return matchesWon / (double)matchesPlayed;
    }
}
