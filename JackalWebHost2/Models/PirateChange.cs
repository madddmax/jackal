using Jackal.Core;

namespace JackalWebHost.Models;

public record PirateChange(Pirate pirate)
{
    public Guid Id = pirate.Id;
    
    public int TeamId = pirate.TeamId;
    
    public TilePosition Position = pirate.Position;
    
    public bool? IsAlive;
    
    public bool? IsDrunk;
    
    public bool? IsInTrap;
}