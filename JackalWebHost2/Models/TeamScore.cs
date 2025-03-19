using Jackal.Core.Domain;

namespace JackalWebHost2.Models;

public class TeamScore(Team team)
{
    public int TeamId = team.Id;
    
    public int Coins = team.Coins;
}