using System;
using System.Linq;

namespace Jackal.Core.Actions
{
    class Moving : IGameAction
    {
        private TilePosition _from;
        private  TilePosition _to;
        private bool _withCoin;

        public Moving(TilePosition from, TilePosition to, bool withCoin = false)
        {
            _to = to;
            _from = from;
            _withCoin = withCoin;
        }

        public GameActionResult Act(Game game, Pirate pirate)
        {
            if (_from == _to) //нет движения
            {
                return GameActionResult.Live;
            }

            var board = game.Board;
            var map = game.Board.Map;

            Team ourTeam = board.Teams[pirate.TeamId];
            var ourShip = ourTeam.Ship;
            
            var target = _to;
            var _targetTile = map[target.Position];
            var _sourceTile = map[_from.Position];

            if (_sourceTile.Type == TileType.Airplane 
                && _from != _to 
                && _sourceTile.Used == false)
            {
                //отмечаем, что мы использовали самолет
                _sourceTile.Used = true;
            }
            
            //открываем клетку, если нужно
            if (_targetTile.Type == TileType.Unknown) //открываем закрытую клетку
            {
                var newTile = board.Generator.GetNext(target.Position);
                board.Map[target.Position] = newTile;
                _targetTile = newTile;

                game.LastActionTurnNo = game.TurnNo;

                if (newTile.Type.RequireImmediateMove())
                {
                    var task = new GetAllAvaliableMovesTask();
                    task.TeamId = pirate.TeamId;
                    task.NoCoinMoving = true;
                    task.FirstSource = _to;
                    task.PreviosSource = _from;

                    var targets = game.Board.GetAllAvailableMoves(task);
                    if (targets.Count == 0)
                    {
                        game.KillPirate(pirate);
                        return GameActionResult.Die;
                    }

                    //мы попали в клетку, где должны сделать ещё свой выбор
                    game.NeedSubTurnPirate = pirate;
                    game.PrevSubTurnDirection = new Direction(_from, _to);
                }
                else if (newTile.Type == TileType.Spinning)
                {
                    _to = new TilePosition(_to.Position, newTile.SpinningCount - 1);
                }
                else if (newTile.Type == TileType.Cannibal)
                {
                    game.KillPirate(pirate);
                    return GameActionResult.Die;
                }
            }

            //проверяем, не попадаем ли мы на чужой корабль - тогда мы погибли
            var enemyShips = game.Board.Teams.Where(x => x != ourTeam).Select(x => x.Ship.Position);
            if (enemyShips.Contains(_to.Position))
            {
                game.KillPirate(pirate);
                return GameActionResult.Die;
            }

            var fromTileLevel = map[_from];
            var targetTileLevel = map[_to];

            //убиваем чужих пиратов
            var enemyPirates = targetTileLevel.Pirates.Where(x => x.TeamId != pirate.TeamId).ToList();
            foreach (var enemyPirate in enemyPirates)
            {
                Team enemyTeam = board.Teams[enemyPirate.TeamId];
                //tile.Coins += enemyPirate.Coins;
                //enemyPirate.Coins = 0;

                if (_targetTile.Type != TileType.Water) //возвращаем врага на его корабль
                {
                    enemyPirate.Position = new TilePosition(enemyTeam.Ship.Position);
                    board.Map[enemyTeam.Ship.Position].Pirates.Add(enemyPirate);
                    targetTileLevel.Pirates.Remove(enemyPirate);
                    enemyPirate.IsInTrap = false;
                    enemyPirate.IsDrunk = false;
                    enemyPirate.DrunkSinceTurnNo = null;
                }
                else //убиаваем совсем
                {
                    game.KillPirate(enemyPirate);
                }
            }
            
            if (_from.Position == ourShip.Position && 
                _targetTile.Type == TileType.Water &&
                Utils.Distance(_from.Position, _to.Position) == 1) 
            {
                //двигаем свой корабль
                var pirateOnShips = map[ourShip.Position].Pirates;
                foreach (var pirateOnShip in pirateOnShips)
                {
                    pirateOnShip.Position = _to;
                    targetTileLevel.Pirates.Add(pirateOnShip);
                }
                ourShip.Position = _to.Position;
                _sourceTile.Pirates.Clear();
            }
            else 
            {
                //двигаем своего пирата
                fromTileLevel.Pirates.Remove(pirate);

                pirate.Position = _to;
                targetTileLevel.Pirates.Add(pirate);
            }

            if (_withCoin)
            {
                if (fromTileLevel.Coins == 0) throw new Exception("No coins");

                fromTileLevel.Coins--;

                if (ourTeam.Ship.Position == _to.Position)  //перенос монеты на корабль
                {
                    ourShip.Coins++;
                    //_pirate.Coins++;

                    game.Scores[pirate.TeamId]++;
                    game.CoinsLeft--;

                    game.LastActionTurnNo = game.TurnNo;
                }
                else if (_targetTile.Type == TileType.Water) // если монета попала в воду, то она тонет
                {
                    game.CoinsLeft--;
                    game.LastActionTurnNo = game.TurnNo;
                }
                else
                {
                    targetTileLevel.Coins++;
                }
            }

            //проводим пьянку для пирата
            switch (_targetTile.Type)
            {
                case TileType.RumBarrel:
                    pirate.DrunkSinceTurnNo = game.TurnNo;
                    pirate.IsDrunk = true;
                    break;
                case TileType.Trap:
                    if (_targetTile.Pirates.Count == 1)
                    {
                        pirate.IsInTrap = true;
                    }
                    else
                    {
                        foreach (Pirate pirateOnTile in _targetTile.Pirates)
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
    }
}