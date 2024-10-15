using Jackal.Core;
using Jackal.Core.Domain;

namespace JackalWebHost2.Models;

public class PirateChange(Pirate pirate)
{
    public Guid Id = pirate.Id;
    
    public PirateType Type = pirate.Type;
    
    public int TeamId = pirate.TeamId;
    
    public LevelPosition Position = new(pirate.Position);
    
    public bool? IsAlive;
    
    public bool? IsDrunk;
    
    public bool? IsInTrap;
    
    public bool? IsInHole;
}