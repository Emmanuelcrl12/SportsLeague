using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
{
    private new readonly LeagueDbContext _context; // Usamos 'new' para evitar el warning de ocultamiento

    public TournamentSponsorRepository(LeagueDbContext context) : base(context)
    {
        _context = context;
    }

    // 1. Obtener por Torneo (Ya lo tenías)
    public async Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentId)
    {
        return await _context.TournamentSponsors
            .Include(ts => ts.Sponsor)
            .Include(ts => ts.Tournament)
            .Where(ts => ts.TournamentId == tournamentId)
            .ToListAsync();
    }

    // 2. Obtener por Sponsor (Faltaba este)
    public async Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId)
    {
        return await _context.TournamentSponsors
            .Include(ts => ts.Sponsor)
            .Include(ts => ts.Tournament)
            .Where(ts => ts.SponsorId == sponsorId)
            .ToListAsync();
    }

    // 3. Eliminar la relación (Faltaba este)
    public async Task DeleteRelationAsync(int sponsorId, int tournamentId)
    {
        var relation = await _context.TournamentSponsors
            .FirstOrDefaultAsync(ts => ts.SponsorId == sponsorId && ts.TournamentId == tournamentId);

        if (relation != null)
        {
            _context.TournamentSponsors.Remove(relation);
            await _context.SaveChangesAsync();
        }
    }
}