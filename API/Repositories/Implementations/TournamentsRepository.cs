using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using API.DTOs;
using API.Entities;
using API.Enums;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace API.Repositories.Implementations;

[SuppressMessage("Performance", "CA1862:Use the \'StringComparison\' method overloads to perform case-insensitive string comparisons")]
[SuppressMessage("ReSharper", "SpecifyStringComparison")]
public class TournamentsRepository(OtrContext context) : RepositoryBase<Tournament>(context), ITournamentsRepository
{
    private readonly OtrContext _context = context;

    public async Task<Tournament?> GetAsync(int id, bool eagerLoad = false)
    {
        if (eagerLoad)
        {
            return await TournamentsBaseQuery().FirstOrDefaultAsync(x => x.Id == id);
        }

        return await base.GetAsync(id);
    }

    public async Task<bool> ExistsAsync(string name, int mode) =>
        await _context.Tournaments.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.Mode == mode);

    public async Task<PlayerTournamentTeamSizeCountDTO> GetPlayerTeamSizeStatsAsync(
        int playerId,
        int mode,
        DateTime dateMin,
        DateTime dateMax
    )
    {
        var participatedTournaments = await _context
            .Tournaments.Where(tournament => tournament.Mode == mode)
            .Include(tournament => tournament.Matches)
            .ThenInclude(match => match.Games)
            .ThenInclude(game => game.MatchScores)
            .Where(tournament =>
                tournament.Matches.Any(match =>
                    match.StartTime >= dateMin
                    && match.StartTime <= dateMax
                    && match.VerificationStatus == 0
                    && match.Games.Any(game =>
                        game.VerificationStatus == 0
                        && game.MatchScores.Any(score => score.PlayerId == playerId && score.IsValid == true)
                    )
                )
            )
            .Select(tournament => new { TournamentId = tournament.Id, tournament.TeamSize })
            .Distinct() // Ensures each tournament is counted once
            .ToListAsync();

        return new PlayerTournamentTeamSizeCountDTO
        {
            Count1v1 = participatedTournaments.Count(x => x.TeamSize == 1),
            Count2v2 = participatedTournaments.Count(x => x.TeamSize == 2),
            Count3v3 = participatedTournaments.Count(x => x.TeamSize == 3),
            Count4v4 = participatedTournaments.Count(x => x.TeamSize == 4),
            CountOther = participatedTournaments.Count(x => x.TeamSize > 4)
        };
    }

    public async Task<IEnumerable<PlayerTournamentMatchCostDTO>> GetPerformancesAsync(
        int count,
        int playerId,
        int mode,
        DateTime dateMin,
        DateTime dateMax,
        bool bestPerformances
    )
    {
        var order = bestPerformances ? "DESC" : "ASC";

        using (DbCommand command = _context.Database.GetDbConnection().CreateCommand())
        {
            var sql = $"""
                SELECT t.id as TournamentId, t.name as TournamentName, AVG(mrs.match_cost) as MatchCost, t.abbreviation AS TournamentAcronym
                								FROM tournaments t
                								INNER JOIN matches m ON m.tournament_id = t.id
                								INNER JOIN match_rating_stats mrs ON mrs.match_id = m.id
                								WHERE mrs.player_id = @playerId AND t.mode = @mode AND m.start_time >= @dateMin AND m.start_time <= @dateMax AND m.verification_status = 0
                								GROUP BY t.id
                								ORDER BY AVG(mrs.match_cost) {order}
                								LIMIT @count
                """;

            command.CommandType = CommandType.Text;
            command.CommandText = sql;

            command.Parameters.Add(new NpgsqlParameter<int>("playerId", playerId));
            command.Parameters.Add(new NpgsqlParameter<int>("mode", mode));
            command.Parameters.Add(
                new NpgsqlParameter<DateTime>("dateMin", NpgsqlDbType.TimestampTz) { Value = dateMin }
            );
            command.Parameters.Add(
                new NpgsqlParameter<DateTime>("dateMax", NpgsqlDbType.TimestampTz) { Value = dateMax }
            );
            command.Parameters.Add(new NpgsqlParameter<int>("count", count));

            await _context.Database.OpenConnectionAsync();

            using (DbDataReader result = await command.ExecuteReaderAsync())
            {
                var results = new List<PlayerTournamentMatchCostDTO>();
                while (await result.ReadAsync())
                {
                    results.Add(
                        new PlayerTournamentMatchCostDTO
                        {
                            PlayerId = playerId,
                            TournamentId = result.GetInt32(0),
                            TournamentName = result.GetString(1),
                            MatchCost = result.GetDouble(2),
                            TournamentAcronym = result.GetString(3),
                            Mode = mode
                        }
                    );
                }

                return results;
            }
        }
    }

    public async Task<int> CountPlayedAsync(
        int playerId,
        int mode,
        DateTime? dateMin = null,
        DateTime? dateMax = null
    )
    {
        dateMin ??= DateTime.MinValue;
        dateMax ??= DateTime.MaxValue;

        return await _context
            .Tournaments.Include(tournament => tournament.Matches)
            .ThenInclude(match => match.Games)
            .ThenInclude(game => game.MatchScores)
            .Where(tournament =>
                tournament.Mode == mode
                && tournament.Matches.Any(match =>
                    match.StartTime >= dateMin
                    && match.StartTime <= dateMax
                    && match.VerificationStatus == (int)MatchVerificationStatus.Verified
                    && match.Games.Any(game =>
                        game.MatchScores.Any(score => score.PlayerId == playerId && score.IsValid == true)
                    )
                )
            )
            .Select(x => x.Id)
            .Distinct()
            .CountAsync();
    }

    private IQueryable<Tournament> TournamentsBaseQuery()
    {
        return _context.Tournaments
            .Include(e => e.Matches)
            .ThenInclude(m => m.Games)
            .ThenInclude(g => g.MatchScores)
            .Include(e => e.Matches)
            .ThenInclude(m => m.Games)
            .ThenInclude(g => g.Beatmap)
            .Include(e => e.SubmittedBy);
    }
}
