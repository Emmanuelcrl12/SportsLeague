
// No uses DTOs aquí, bórralos.

namespace SportsLeague.Domain.Interfaces.Services;
using SportsLeague.Domain.Entities;

public interface ISponsorService
{
    Task<IEnumerable<Sponsor>> GetAllAsync();
    Task<Sponsor?> GetByIdAsync(int id);
    Task<Sponsor> CreateAsync(Sponsor sponsor); // Usa la entidad Sponsor
    Task UpdateAsync(int id, Sponsor sponsor);  // Usa la entidad Sponsor
    Task DeleteAsync(int id);
    
    // Nombres exactos según la lógica de vinculación
    Task LinkToTournamentAsync(TournamentSponsor tournamentSponsor);
    Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId);
    Task UnlinkFromTournamentAsync(int sponsorId, int tournamentId);
}