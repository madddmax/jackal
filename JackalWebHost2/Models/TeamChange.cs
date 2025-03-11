namespace JackalWebHost2.Models;

public class TeamChange(DrawTeam team)
{
    public int Id = team.Id;
    public int Coins = team.Coins;
}