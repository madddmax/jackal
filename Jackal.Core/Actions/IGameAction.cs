namespace Jackal.Actions
{
    public interface IGameAction
    {
        GameActionResult Act(Game game,Pirate pirate);
    }
}