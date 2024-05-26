namespace Jackal.Actions
{
    class Dying : IGameAction
    {
        public Dying()
        {
        }

        public GameActionResult Act(Game game,Pirate pirate)
        {
            game.KillPirate(pirate);

            return GameActionResult.Die;
        }
    }
}