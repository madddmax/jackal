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
            game.Board.Generator.Swap(from.Position, to.Position);
            
            // меняем клетки местами
            var fromTile = map[from.Position];
            map[from.Position] = new Tile(from.Position, map[to.Position]);
            map[to.Position] = new Tile(to.Position, fromTile);
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