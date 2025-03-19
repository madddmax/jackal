using Jackal.Core.Domain;

namespace JackalWebHost2.Models;

public class DrawTeam(Team team, bool isHuman)
{
    public readonly int Id = team.Id;
    
    public readonly string Name = team.Name;
    
    public readonly bool IsHuman = isHuman;
    
    public DrawPosition Ship = new(team.ShipPosition);
}