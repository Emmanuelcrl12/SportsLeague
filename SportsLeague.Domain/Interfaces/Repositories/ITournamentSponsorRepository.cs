using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface ITournamentSponsorRepository : IGenericRepository<TournamentSponsor>
{
    // Agrega estas dos líneas:
    Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId);
    Task DeleteRelationAsync(int sponsorId, int tournamentId);
}