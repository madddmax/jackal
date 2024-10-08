namespace Jackal.Core.Actions;

public class QuakeAction(TilePosition from, TilePosition to) : IGameAction
{
    public GameActionResult Act(Game game, Pirate pirate)
    {
        var map = game.Board.Map;
        
        // выбираем вторую клетку для разлома
        if (game.SubTurnQuakePhase == 1)
        {
            game.SubTurnQuakePhase = 0;
            (map[from.Position], map[to.Position]) = (map[to.Position], map[from.Position]);
        }
        
        // выбираем первую клетку для разлома
        if (game.SubTurnQuakePhase == 2)
        {
            game.SubTurnQuakePhase = 1;
            game.NeedSubTurnPirate = pirate;
            game.PrevSubTurnPosition = to;
        }

        return GameActionResult.Live;
    }
}