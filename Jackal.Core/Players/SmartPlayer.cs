using System;
using System.Collections.Generic;
using System.Linq;

namespace Jackal.Core.Players
{
    public class SmartPlayer : IPlayer
    {
        private Random Rnd;

        public void OnNewGame()
        {
            Rnd = new Random(1);
        }

        public void SetHumanMove(int moveNum, Guid? pirateId)
        {
            throw new NotImplementedException();
        }

        public (int moveNum, Guid? pirateId) OnMove(GameState gameState)
        {
            Board board=gameState.Board;
            Move[] availableMoves = gameState.AvailableMoves;
            int teamId = gameState.TeamId;

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
                foreach (Move move in availableMoves)
                {
                    if (move.WithCoins)
                    {
                        //int currentDistance = Distance(ship.Position, move.Pirate.Position);
                        int newDistance = Distance(ship.Position, move.To.Position);
                        list.Add(new Tuple<int, Move>(newDistance, move));
                    }
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
                foreach (var tile in board.AllTiles(x => x.Type != TileType.Water&&x.Coins>0))
                {
                        tilesWithGold.Add(tile);
                }
                if (tilesWithGold.Count > 0) //на карте есть золото
                {
                    List<Tuple<int, Move>> list = new List<Tuple<int, Move>>();

                    foreach (Move move in availableMoves)
                    {
                        int newMinDistance = MinDistance(tilesWithGold.ConvertAll(x => x.Position), move.To.Position);
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

                    foreach (Move move in availableMoves)
                    {
                        int newMinDistance = MinDistance(tilesWithUnknown.ConvertAll(x => x.Position), move.To.Position);
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
                if (availableMoves[i]== resultMove)
                    return (i, null);
            }
            return (0, null);
        }

        private int MinDistance(List<Position> positions, Position to)
        {
            return positions.ConvertAll(x => Distance(x, to)).Min();
        }

        private bool IsEnemyPosition(Position to, Board board, int teamId)
        {
            var occupationTeamId = board.Map[to].OccupationTeamId;
            if (occupationTeamId.HasValue && board.Teams[teamId].Enemies.ToList().Exists(x => x == occupationTeamId.Value)) return true;
            return false;
        }

        int Distance(Position pos1, Position pos2)
        {
            int deltaX = Math.Abs(pos1.X - pos2.X);
            int deltaY = Math.Abs(pos1.Y - pos2.Y);
            int totalDelta = Math.Max(deltaX, deltaY);
            return totalDelta;
        }

        private bool TargetIsShip(Board board, int teamId, Move move)
        {
            var ship = board.Teams[teamId].Ship;
            return (ship.Position == move.To.Position);
        }
    }
}