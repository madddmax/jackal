using Jackal.Core.Domain;

namespace Jackal.Core.Actions;

internal class RespawnAction : IGameAction
{
    public void Act(Game game, Pirate pirate)
    {
        game.AddPirate(pirate.TeamId, pirate.Position, PirateType.Usual);
    }
}