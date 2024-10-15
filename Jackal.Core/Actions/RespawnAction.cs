using Jackal.Core.Domain;

namespace Jackal.Core.Actions;

internal class RespawnAction : IGameAction
{
    public GameActionResult Act(Game game, Pirate pirate)
    {
        game.AddPirate(pirate.TeamId, pirate.Position, PirateType.Usual);
            
        return GameActionResult.Live;
    }
}