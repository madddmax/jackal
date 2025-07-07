using System;
using Jackal.Core.Domain;

namespace Jackal.Core.Actions;

public class DrinkRumBottleAction : IGameAction
{
    public void Act(Game game, Pirate pirate)
    {
        Board board = game.Board;
        Team ourTeam = board.Teams[pirate.TeamId];
        Team? allyTeam = ourTeam.AllyTeamId.HasValue 
            ? board.Teams[ourTeam.AllyTeamId.Value] 
            : null;

        if (ourTeam.RumBottles == 0) 
            throw new Exception("No rum bottles");
        
        ourTeam.RumBottles -= 1;
        if (allyTeam != null)
            allyTeam.RumBottles -= 1;
    }
}