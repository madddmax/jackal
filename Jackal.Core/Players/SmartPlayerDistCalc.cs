using System;
using System.Collections.Generic;
using System.Linq;

namespace Jackal.Core.Players
{
    public class SmartPlayerDistCalc : IPlayer
    {
        private Random Rnd;

        public void OnNewGame()
        {
            Rnd = new Random(1);
        }

        public void SetHumanMove(int moveNum)
        {
            throw new NotImplementedException();
        }

        public int OnMove(GameState gameState)
        {
            Board board = gameState.Board;
            Move[] availableMoves = gameState.AvailableMoves;
            int teamId = gameState.TeamId;

			_distanceCalculator = new DistanceCalc(board, 1.2);

            List<Move> goodMoves = new List<Move>();
            foreach (Move move in availableMoves)
            {
                //золото на корабль
                if (move.WithCoins && TargetIsShip(board, teamId, move)) goodMoves.Add(move);
            }
            if (goodMoves.Count == 0)
            {
                //перемещаем золото ближе к кораблю
                var ship = board.Teams[teamId].Ship;
                List<Tuple<int, Move>> list = new List<Tuple<int, Move>>();
                foreach (Move move in availableMoves
                    .Where(x => x.WithCoins)
                    .Where(x => IsEnemyNear(x.To.Position, board, teamId) == false))
                {
                    //int currentDistance = Distance(ship.Position, move.Pirate.Position);
                    int newDistance = Distance(move.To.Position, ship.Position, teamId, true);
                    list.Add(new Tuple<int, Move>(newDistance, move));
                }
                if (list.Count > 0)
                {
                    int minDistance = list.Min(x => x.Item1);
                    goodMoves = list.Where(x => x.Item1 == minDistance).Select(x => x.Item2).ToList();
                }
            }
            if (goodMoves.Count == 0) //уничтожаем врага, если он рядом
            {
                foreach (Move move in availableMoves)
                {
                    if (IsEnemyPosition(move.To.Position, board, teamId)) goodMoves.Add(move);
                }
            }
            if (goodMoves.Count == 0)
            {
                List<Tile> tilesWithGold = new List<Tile>();
                //если на карте есть открытое золото, то идем к нему
                foreach (var tile in board.AllTiles(x => x.Type != TileType.Water && x.Coins > 0))
                {
                    tilesWithGold.Add(tile);
                }
                if (tilesWithGold.Count > 0) //на карте есть золото
                {
                    List<Tuple<int, Move>> list = new List<Tuple<int, Move>>();

                    foreach (Move move in availableMoves
                        .Where(x => x.WithCoins == false)
                        .Where(x => IsEnemyNear(x.To.Position, board, teamId) == false))
                    {
                        int newMinDistance = MinDistance(move.To.Position, tilesWithGold.ConvertAll(x => x.Position), teamId);
                        list.Add(new Tuple<int, Move>(newMinDistance, move));
                    }

                    if (list.Count > 0)
                    {
                        int minDistance = list.Min(x => x.Item1);
                        goodMoves = list.Where(x => x.Item1 == minDistance).Select(x => x.Item2).ToList();
                    }
                }

            }

            if (goodMoves.Count == 0)
            {
                List<Tile> tilesWithUnknown = new List<Tile>();
                //если на карте есть неоткрытый участок золото, то идем к нему
                foreach (var tile in board.AllTiles(x => x.Type == TileType.Unknown))
                {
                    tilesWithUnknown.Add(tile);
                }
                if (tilesWithUnknown.Count > 0) //на карте есть неоткрытый участок 
                {
                    List<Tuple<int, Move>> list = new List<Tuple<int, Move>>();

                    foreach (Move move in availableMoves
                        .Where(x => x.WithCoins == false)
                        .Where(x => IsEnemyNear(x.To.Position, board, teamId) == false))
                    {
                        int newMinDistance = MinDistance(move.To.Position, tilesWithUnknown.ConvertAll(x => x.Position), teamId);
                        list.Add(new Tuple<int, Move>(newMinDistance, move));
                    }
                    if (list.Count > 0)
                    {
                        int minDistance = list.Min(x => x.Item1);
                        goodMoves = list.Where(x => x.Item1 == minDistance).Select(x => x.Item2).ToList();
                    }
                }
            }

            //выбираем доступный ход
            if (goodMoves.Count == 0)
            {
                goodMoves.AddRange(availableMoves);
            }

            var resultMove = goodMoves[Rnd.Next(goodMoves.Count)];
            for (int i = 0; i < availableMoves.Length; i++)
            {
                if (availableMoves[i] == resultMove)
                    return i;
            }
            return 0;
        }

        private bool IsEnemyNear(Position to, Board board, int ourTeamId)
        {
            if (board.Map[to].Type == TileType.Water) return false;

            List<Pirate> enemyList = board.Teams.Where(t=>t.Id!=ourTeamId).SelectMany(t=>t.Pirates).ToList();

	        return enemyList.Any(x => Distance(x.Position.Position , to, x.TeamId) == 1 );
		}

		private int MinDistance(List<Position> positions, Position to, int teamId, bool withGold = false)
        {
			return positions.ConvertAll(x => Distance(x, to, teamId, withGold)).Min();
        }

		private int MinDistance(Position from, List<Position> positionsTo, int teamId, bool withGold = false)
		{
			return positionsTo.ConvertAll(x => Distance(from, x, teamId, withGold)).Min();
		}

        private bool IsEnemyPosition(Position to, Board board, int teamId)
        {
            var occupationTeamId = board.Map[to].OccupationTeamId;
            if (occupationTeamId.HasValue && board.Teams[teamId].Enemies.ToList().Exists(x => x == occupationTeamId.Value)) return true;
            return false;
        }

	    private DistanceCalc _distanceCalculator;
        int Distance(Position pos1, Position pos2, int teamId, bool withGold = false)
        {
	        var res = _distanceCalculator.Distance(pos1, pos2, teamId, withGold);
	        return res == null ? 10000 : (int) res.Value;
        }

        private bool TargetIsShip(Board board, int teamId, Move move)
        {
            var ship = board.Teams[teamId].Ship;
            return (ship.Position == move.To.Position);
        }
    }
}