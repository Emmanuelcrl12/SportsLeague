using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using System.Linq;

namespace SportsLeague.DataAccess.Repositories;

public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository
{
    private readonly LeagueDbContext _context;

    public SponsorRepository(LeagueDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByNameAsync(string name) =>
        await _context.Sponsors
            .AnyAsync(s => s.Name.ToLower() == name.ToLower().Trim());

    public async Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId)
    {
        return await _context.TournamentSponsors
            .Include(ts => ts.Tournament)
            .Include(ts => ts.Sponsor)
            .Where(ts => ts.SponsorId == sponsorId)
            .ToListAsync();
    }
}
