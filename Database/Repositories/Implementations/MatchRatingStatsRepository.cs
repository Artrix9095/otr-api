using System.Diagnostics.CodeAnalysis;
using Database.Entities.Processor;
using Database.Enums;
using Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories.Implementations;

[SuppressMessage("Performance", "CA1862:Use the \'StringComparison\' method overloads to perform case-insensitive string comparisons")]
[SuppressMessage("ReSharper", "SpecifyStringComparison")]
public class MatchRatingStatsRepository(OtrContext context) : RepositoryBase<RatingAdjustment>(context), IMatchRatingStatsRepository
{
    private readonly OtrContext _context = context;

    public async Task<IEnumerable<IEnumerable<RatingAdjustment>>> GetForPlayerAsync(
        int playerId,
        Ruleset ruleset,
        DateTime? dateMin = null,
        DateTime? dateMax = null
    )
    {
        dateMin ??= DateTime.MinValue;
        dateMax ??= DateTime.MaxValue;

        return await _context.RatingAdjustments
            .Where(x =>
                x.PlayerId == playerId
                && x.AdjustmentType == RatingAdjustmentType.Match
                && x.Match!.Tournament.Ruleset == ruleset
                && x.Match.StartTime >= dateMin
                && x.Match.StartTime <= dateMax
            )
            .Include(x => x.Match)
            .ThenInclude(x => x!.Tournament)
            .GroupBy(x => x.Match!.StartTime.Date)
            .ToListAsync();
    }

    public async Task TruncateAsync() =>
        await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE match_rating_stats RESTART IDENTITY");

    public async Task<IEnumerable<RatingAdjustment>> TeammateRatingStatsAsync(
        int playerId,
        int teammateId,
        Ruleset ruleset,
        DateTime dateMin,
        DateTime dateMax
    ) =>
        await _context.RatingAdjustments
            .Where(mrs => mrs.PlayerId == playerId)
            .Where(mrs =>
                _context.PlayerMatchStats.Any(pms =>
                    pms.PlayerId == mrs.PlayerId
                    && pms.TeammateIds.Contains(teammateId)
                    && pms.Match.Tournament.Ruleset == ruleset
                    && pms.Match.StartTime >= dateMin
                    && pms.Match.StartTime <= dateMax
                )
            )
            .Distinct()
            .ToListAsync();

    public async Task<IEnumerable<RatingAdjustment>> OpponentRatingStatsAsync(
        int playerId,
        int opponentId,
        Ruleset ruleset,
        DateTime dateMin,
        DateTime dateMax
    ) =>
        await _context.RatingAdjustments
            .Where(ra => ra.PlayerId == playerId)
            .Where(ra =>
                _context.PlayerMatchStats.Any(pms =>
                    pms.PlayerId == ra.PlayerId
                    && pms.OpponentIds.Contains(opponentId)
                    && pms.Match.Tournament.Ruleset == ruleset
                    && pms.Match.StartTime >= dateMin
                    && pms.Match.StartTime <= dateMax
                )
            )
            .Distinct()
            .ToListAsync();
}
