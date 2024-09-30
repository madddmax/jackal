using System;
using System.Collections.Generic;
using System.Linq;

namespace Jackal.Core.Actions;

internal class Moving(TilePosition from, TilePosition to, TilePosition prev, bool withCoin = false) : IGameAction
{
    public GameActionResult Act(Game game, Pirate pirate)
    {
        // нет движения
        if (from == to)
        {
            // придерживаем самолет
            return GameActionResult.Live;
        }
            
        Board board = game.Board;
        Map map = game.Board.Map;

        Team ourTeam = board.Teams[pirate.TeamId];
        Ship ourShip = ourTeam.Ship;
            
        Tile targetTile = map[to.Position];
        Tile sourceTile = map[from.Position];
        Tile prevTile = map[prev.Position];
            
        // открываем закрытую клетку
        bool newTile = false;
        if (targetTile.Type == TileType.Unknown)
        {
            targetTile = board.Generator.GetNext(to.Position);
            board.Map[to.Position] = targetTile;
            game.LastActionTurnNo = game.TurnNo;
            newTile = true;
        }
            
        // воздушный шар переносит сразу на наш корабль
        if (targetTile.Type == TileType.Balloon)
        {
            to = new TilePosition(ourShip.Position);
        }
            
        // пушка выстреливает сразу в воду
        if (targetTile.Type == TileType.Cannon)
        {
            to = GetCannonFly(targetTile.Direction, to.Position, board.MapSize);
        }
            
        // ходим по клетке вертушке
        if (newTile && targetTile.Type == TileType.Spinning)
        {
            to = new TilePosition(to.Position, targetTile.SpinningCount - 1);
        }
            
        // нашли карамбу
        if (targetTile is { Type: TileType.Caramba, Used: false })
        {
            // проходим по всем командам и собираем пиратов на кораблях
            foreach (var team in board.Teams)
            {
                foreach (var pirateOnMap in team.Pirates)
                {
                    if (pirateOnMap.Position.Position == team.Ship.Position)
                        continue;

                    // возвращаем пирата на его корабль
                    game.MovePirateToTheShip(pirateOnMap);
                }
            }
                
            to = new TilePosition(ourShip.Position);
            targetTile.Used = true;
        }
            
        // нашли Бен Ганна не маяком
        if (targetTile is { Type: TileType.BenGunn, Used: false } && game.SubTurnLighthouseViewCount == 0)
        {
            game.AddPirate(pirate.TeamId, to, PirateType.BenGunn);
            targetTile.Used = true;
        }
            
        // просматриваем карту с маяка,
        // перезатираем просматриваемую клетку текущей позицией пирата,
        // важно вызвать после всех установок поля to
        if (game.SubTurnLighthouseViewCount > 0)
        {
            game.SubTurnLighthouseViewCount--;
            to = pirate.Position;
        }
            
        // нашли маяк
        if (targetTile is { Type: TileType.Lighthouse, Used: false })
        {
            var unknownTilesCount = game.Board.AllTiles(x => x.Type == TileType.Unknown).Count();
            var remainedTilesViewCount = unknownTilesCount - game.SubTurnLighthouseViewCount;
            game.SubTurnLighthouseViewCount += remainedTilesViewCount < 4 ? remainedTilesViewCount : 4;
                
            targetTile.Used = true;
        }
            
        targetTile = map[to.Position];
        TileLevel targetTileLevel = map[to];
        TileLevel fromTileLevel = map[from];
            
        if (from.Position == ourShip.Position && 
            targetTile.Type == TileType.Water &&
            Board.GetPossibleShipMoves(ourShip.Position, game.Board.MapSize).Contains(to.Position)) 
        {
            // двигаем свой корабль
            var pirateOnShips = map[ourShip.Position].Pirates;
            foreach (var pirateOnShip in pirateOnShips)
            {
                pirateOnShip.Position = to;
                targetTileLevel.Pirates.Add(pirateOnShip);
            }
            ourShip.Position = to.Position;
            sourceTile.Pirates.Clear();
        }
        else
        {
            // двигаем своего пирата
            fromTileLevel.Pirates.Remove(pirate);

            pirate.Position = to;
            targetTileLevel.Pirates.Add(pirate);
        }

        if (game.SubTurnLighthouseViewCount > 0 ||
            targetTile is { Used: false, Type: TileType.Airplane })
        {
            game.NeedSubTurnPirate = pirate;
            game.PrevSubTurnPosition = prev;
        }
        
        if (targetTile.Type == TileType.Hole && !game.SubTurnFallingInTheHole)
        {
            var holeTiles = board.AllTiles(x => x.Type == TileType.Hole).ToList();
            if(holeTiles.Count == 1)
            {
                pirate.IsInHole = true;
            }
            else if (newTile && holeTiles.Count == 2)
            {
                var pirates = new List<Pirate>(holeTiles[1].Pirates);
                foreach (var movedPirate in holeTiles[0].Pirates)
                {
                    game.MovePirateToPosition(movedPirate, holeTiles[1].Position);
                }

                foreach (var movedPirate in pirates)
                {
                    game.MovePirateToPosition(movedPirate, holeTiles[0].Position);
                }
            }
            else
            {
                game.NeedSubTurnPirate = pirate;
                game.PrevSubTurnPosition = prev;
                game.SubTurnFallingInTheHole = true;
            }
        }

        if (newTile && targetTile.Type is TileType.Arrow or TileType.Horse or TileType.Ice or TileType.Crocodile)
        {
            var airplaneFlying = targetTile.Type is TileType.Ice or TileType.Crocodile &&
                                 (prevTile is { Type: TileType.Airplane, Used: false } ||
                                  game.SubTurnAirplaneFlying);

            AvailableMovesTask task = new AvailableMovesTask(pirate.TeamId, to, prev);
            List<AvailableMove> moves = game.Board.GetAllAvailableMoves(
                task, task.Source, task.Prev, airplaneFlying, 0, false
            );

            if (moves.Count == 0)
            {
                game.KillPirate(pirate);
                return GameActionResult.Die;
            }
                
            game.NeedSubTurnPirate = pirate;
            game.PrevSubTurnPosition = prev;
            game.SubTurnAirplaneFlying = airplaneFlying;
        }
        else
        {
            game.SubTurnAirplaneFlying = false;
        }
            
        // отмечаем, что мы использовали самолет
        if (from != to)
        {
            if(sourceTile is { Type: TileType.Airplane, Used: false })
                sourceTile.Used = true;
                
            if(prevTile is { Type: TileType.Airplane, Used: false })
                prevTile.Used = true;
        }

        // проверяем, не попадаем ли мы на чужой корабль - тогда мы погибли
        IEnumerable<Position> enemyShips = game.Board.Teams
            .Where(x => x != ourTeam)
            .Select(x => x.Ship.Position);
            
        if (enemyShips.Contains(to.Position))
        {
            game.KillPirate(pirate);
            return GameActionResult.Die;
        }

        // убиваем чужих пиратов
        var enemyPirates = targetTileLevel.Pirates
            .Where(x => x.TeamId != pirate.TeamId && !x.IsInHole)
            .ToList();

        foreach (var enemyPirate in enemyPirates)
        {
            if (targetTile.Type == TileType.Jungle)
                continue;
            
            if (targetTile.Type == TileType.Water)
                game.KillPirate(enemyPirate);

            game.MovePirateToTheShip(enemyPirate);
        }

        if (withCoin)
        {
            if (fromTileLevel.Coins == 0) 
                throw new Exception("No coins");

            fromTileLevel.Coins--;

            if (ourTeam.Ship.Position == to.Position)
            {
                // перенос монеты на корабль
                ourShip.Coins++;

                game.Scores[pirate.TeamId]++;
                game.CoinsLeft--;

                game.LastActionTurnNo = game.TurnNo;
            }
            else if (targetTile.Type == TileType.Water)
            {
                // если монета попала в воду, то она тонет
                game.CoinsLeft--;
                game.LastActionTurnNo = game.TurnNo;
            }
            else
            {
                targetTileLevel.Coins++;
            }
        }
            
        switch (targetTile.Type)
        {
            case TileType.RumBarrel:
                // проводим пьянку для пирата
                pirate.DrunkSinceTurnNo = game.TurnNo;
                pirate.IsDrunk = true;
                break;
            case TileType.Trap:
                if (targetTile.Pirates.Count == 1)
                {
                    pirate.IsInTrap = true;
                }
                else
                {
                    foreach (Pirate pirateOnTile in targetTile.Pirates)
                    {
                        pirateOnTile.IsInTrap = false;
                    }
                }
                break;
            case TileType.Cannibal:
                game.KillPirate(pirate);
                return GameActionResult.Die;
        }

        return GameActionResult.Live;
    }
        
    private static TilePosition GetCannonFly(int direction, Position pos, int mapSize) =>
        direction switch
        {
            // вверх
            0 => new TilePosition(pos.X, mapSize - 1),
            // вправо
            1 => new TilePosition(mapSize - 1, pos.Y),
            // вниз
            2 => new TilePosition(pos.X, 0),
            // влево
            _ => new TilePosition(0, pos.Y)
        };
}