namespace Jackal.Core.Domain;

public record Team
{
    public readonly int Id;
    
    public readonly string Name;
    
    public readonly Ship Ship;
    
    public Pirate[] Pirates;
    
    public int[] Enemies;
    
    public int Coins;

    public Team(int id, string name, Ship ship, Pirate[] pirates)
    {
        Id = id;
        Name = name;
        Ship = ship;
        Pirates = pirates;
    }
}