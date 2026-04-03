using AutoMapper;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;

namespace SportsLeague.API.Services;

public class SponsorService : ISponsorService
{
    private readonly ISponsorRepository _repository;
    private readonly ITournamentSponsorRepository _tsRepository;
    private readonly IMapper _mapper;

    public SponsorService(
        ISponsorRepository repository, 
        ITournamentSponsorRepository tsRepository,
        IMapper mapper)
    {
        _repository = repository;
        _tsRepository = tsRepository;
        _mapper = mapper;
    }

    // --- CRUD BÁSICO ---

    public async Task<IEnumerable<Sponsor>> GetAllAsync() 
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Sponsor?> GetByIdAsync(int id) 
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Sponsor> CreateAsync(Sponsor sponsor)
    {
        if (await _repository.ExistsByNameAsync(sponsor.Name))
            throw new InvalidOperationException("No se puede crear un Sponsor con Name duplicado");

        if (string.IsNullOrEmpty(sponsor.ContactEmail) || !sponsor.ContactEmail.Contains("@"))
            throw new InvalidOperationException("ContactEmail debe ser un formato válido");

        sponsor.CreatedAt = DateTime.Now;
        return await _repository.CreateAsync(sponsor);
    }

    public async Task UpdateAsync(int id, Sponsor sponsor)
    {
        var existingSponsor = await _repository.GetByIdAsync(id);
        if (existingSponsor == null) throw new KeyNotFoundException("Sponsor no encontrado");

        existingSponsor.Name = sponsor.Name;
        existingSponsor.ContactEmail = sponsor.ContactEmail;
        existingSponsor.Phone = sponsor.Phone;
        existingSponsor.WebsiteUrl = sponsor.WebsiteUrl;
        existingSponsor.Category = sponsor.Category;
        existingSponsor.UpdatedAt = DateTime.Now;

        await _repository.UpdateAsync(existingSponsor);
    }

    public async Task DeleteAsync(int id) 
    {
        await _repository.DeleteAsync(id);
    }

    // --- RELACIÓN N:M (TournamentSponsor) ---

    public async Task LinkToTournamentAsync(TournamentSponsor tournamentSponsor)
    {
        if (tournamentSponsor.ContractAmount <= 0)
            throw new InvalidOperationException("ContractAmount debe ser mayor a 0");

        tournamentSponsor.JoinedAt = DateTime.Now;
        await _tsRepository.CreateAsync(tournamentSponsor);
    }

    public async Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId)
    {
        return await _tsRepository.GetBySponsorIdAsync(sponsorId);
    }

    public async Task UnlinkFromTournamentAsync(int sponsorId, int tournamentId)
    {
        await _tsRepository.DeleteRelationAsync(sponsorId, tournamentId);
    }
}