using System;
using Jackal.Core.Domain;

namespace Jackal.Core.Actions;

internal class MovingWithBigCoinAction(TilePosition from, TilePosition to, TilePosition prev) : IGameAction
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

        if (fromTileLevel.BigCoins == 0) 
            throw new Exception("No big coins");

        fromTileLevel.BigCoins--;

        if (ourTeam.ShipPosition == to.Position ||
            (allyTeam != null &&
             allyTeam.ShipPosition == to.Position))
        {
            // перенос монеты на корабль
            ourTeam.Coins += Constants.BigCoinValue;
            if (allyTeam != null)
                allyTeam.Coins += Constants.BigCoinValue;
            
            game.CoinsOnMap -= Constants.BigCoinValue;
            game.LastActionTurnNumber = game.TurnNumber;
        }
        else if (targetTile.Type == TileType.Water)
        {
            // монета в воде - тонет
            game.CoinsOnMap -= Constants.BigCoinValue;
            game.LostCoins += Constants.BigCoinValue;
            game.LastActionTurnNumber = game.TurnNumber;
        }
        else if (targetTile.Type == TileType.Cannibal)
        {
            // монета на людоеде - пропадает т.к. Пятница не реализован
            game.CoinsOnMap -= Constants.BigCoinValue;
            game.LostCoins += Constants.BigCoinValue;
            game.LastActionTurnNumber = game.TurnNumber;
        }
        else
        {
            targetTileLevel.BigCoins++;
        }

        return GameActionResult.Live;
    }
}