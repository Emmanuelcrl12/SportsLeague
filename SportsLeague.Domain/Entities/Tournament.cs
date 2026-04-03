


namespace SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;


public class Tournament : AuditBase

{

public string Name { get; set; } = string.Empty;

public string Season { get; set; } = string.Empty;

public DateTime StartDate { get; set; }

public DateTime EndDate { get; set; }

public TournamentStatus Status { get; set; } = TournamentStatus.Pending;


// Navigation Properties

public ICollection<TournamentTeam> TournamentTeams { get; set; } = new List<TournamentTeam>();

}