namespace Jackal.Core;

public class Team
{
    public int Id;
    public string Name;
    public Ship Ship;
    public Pirate[] Pirates;
    public int[] Enemies;

    public Team(int id, string name, Ship ship, Pirate[] pirates)
    {
        Id = id;
        Name = name;
        Ship = ship;
        Pirates = pirates;
    }
}