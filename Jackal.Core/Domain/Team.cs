namespace Jackal.Core.Domain;

public record Team
{
    public readonly int Id;
    
    public readonly string Name;
    
    public Position ShipPosition;
    
    public Pirate[] Pirates;
    
    public int[] Enemies;
    
    public int Coins;

    public Team(int id, string name, Position shipPosition, Pirate[] pirates)
    {
        Id = id;
        Name = name;
        ShipPosition = shipPosition;
        Pirates = pirates;
    }
}