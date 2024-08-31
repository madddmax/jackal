namespace Jackal.Core.Actions
{
    internal class Respawn : IGameAction
    {
        public GameActionResult Act(Game game, Pirate pirate)
        {
            game.AddPirate(pirate.TeamId, pirate.Position, PirateType.Usual);
            
            return GameActionResult.Live;
        }
    }
}