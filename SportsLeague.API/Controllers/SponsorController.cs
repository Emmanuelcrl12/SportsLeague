using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using SportsLeague.Domain.Interfaces.Services;
using SportsLeague.Domain.Entities;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SponsorController : ControllerBase
{
    private readonly ISponsorService _service;
    private readonly IMapper _mapper;

    public SponsorController(ISponsorService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        // Mapeamos la lista de entidades a una lista de ResponseDTOs
        return Ok(_mapper.Map<IEnumerable<SponsorResponseDTO>>(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        
        return Ok(_mapper.Map<SponsorResponseDTO>(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(SponsorRequestDTO dto)
    {
        // 1. Convertimos el DTO que llega a una Entidad
        var sponsorEntity = _mapper.Map<Sponsor>(dto);
        
        // 2. Llamamos al servicio pasando la Entidad
        var result = await _service.CreateAsync(sponsorEntity);
        
        // 3. Convertimos el resultado a un ResponseDTO para el cliente
        var response = _mapper.Map<SponsorResponseDTO>(result);
        
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SponsorRequestDTO dto)
    {
        // Convertimos el DTO a Entidad para que el servicio la procese
        var sponsorEntity = _mapper.Map<Sponsor>(dto);
        
        await _service.UpdateAsync(id, sponsorEntity);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    // 🔥 N:M - Relación con Torneos

    [HttpGet("{id}/tournaments")]
    public async Task<IActionResult> GetTournaments(int id)
    {
        var result = await _service.GetTournamentsBySponsorAsync(id);
        // Mapeamos la tabla intermedia a su DTO de respuesta
        return Ok(_mapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(result));
    }

    [HttpPost("{id}/tournaments")]
    public async Task<IActionResult> LinkToTournament(int id, TournamentSponsorRequestDTO dto)
    {
        // 1. Mapeamos a la entidad intermedia
        var link = _mapper.Map<TournamentSponsor>(dto);
        link.SponsorId = id; // Aseguramos que el ID venga de la URL

        // 2. El servicio procesa la vinculación
        await _service.LinkToTournamentAsync(link);
        
        // 3. Según la guía, devolvemos el objeto creado (puedes mapearlo si tienes un DTO de respuesta)
        return Created("", _mapper.Map<TournamentSponsorResponseDTO>(link));
    }

    [HttpDelete("{id}/tournaments/{tid}")]
    public async Task<IActionResult> Unlink(int id, int tid)
    {
        await _service.UnlinkFromTournamentAsync(id, tid);
        return NoContent();
    }
}