using System;
using Jackal.Core.Domain;

namespace Jackal.Core.Actions;

internal class MovingWithCoinAction(TilePosition from, TilePosition to, TilePosition prev) : IGameAction
{
    public GameActionResult Act(Game game, Pirate pirate)
    {
        var movingAction = new MovingAction(from, to, prev);
        movingAction.Act(game, pirate);
        to = movingAction.To;
        
        Board board = game.Board;
        Map map = board.Map;

        Team ourTeam = board.Teams[pirate.TeamId];
        Team? allyTeam = ourTeam.AllyTeamId.HasValue 
            ? board.Teams[ourTeam.AllyTeamId.Value] 
            : null;
            
        Tile targetTile = map[to.Position];
        TileLevel targetTileLevel = map[to];
        TileLevel fromTileLevel = map[from];

        if (fromTileLevel.Coins == 0) 
            throw new Exception("No coins");

        fromTileLevel.Coins--;

        if (ourTeam.ShipPosition == to.Position ||
            (allyTeam != null &&
             allyTeam.ShipPosition == to.Position))
        {
            // перенос монеты на корабль
            ourTeam.Coins++;
            if (allyTeam != null)
                allyTeam.Coins++;
            
            game.CoinsOnMap--;
            game.LastActionTurnNo = game.TurnNo;
        }
        else if (targetTile.Type == TileType.Water)
        {
            // монета в воде - тонет
            game.CoinsOnMap--;
            game.LostCoins++;
            game.LastActionTurnNo = game.TurnNo;
        }
        else if (targetTile.Type == TileType.Cannibal)
        {
            // монета на людоеде - пропадает т.к. Пятница не реализован
            game.CoinsOnMap--;
            game.LostCoins++;
            game.LastActionTurnNo = game.TurnNo;
        }
        else
        {
            targetTileLevel.Coins++;
        }

        return GameActionResult.Live;
    }
}