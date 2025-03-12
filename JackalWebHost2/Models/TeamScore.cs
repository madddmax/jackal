namespace JackalWebHost2.Models;

public class TeamScore(DrawTeam team)
{
    public int TeamId = team.Id;
    public int Coins = team.Coins;
}