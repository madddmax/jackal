using System;
using System.Collections.Generic;
using System.Linq;

namespace Jackal.Core.Players
{
    public class SmartPlayer2 : IPlayer
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
            Board board = gameState.Board;
            Move[] availableMoves = gameState.AvailableMoves;
            int teamId = gameState.TeamId;

            List<Move> goodMoves = new List<Move>();
            foreach (Move move in availableMoves)
            {
                //çîëîòî íà êîðàáëü
                if (move.WithCoins && TargetIsShip(board, teamId, move)) goodMoves.Add(move);
            }
            if (goodMoves.Count == 0)
            {
                //ïåðåìåùàåì çîëîòî áëèæå ê êîðàáëþ
                var ship = board.Teams[teamId].Ship;
                List<Tuple<int, Move>> list = new List<Tuple<int, Move>>();
                foreach (Move move in availableMoves
                    .Where(x => x.WithCoins)
                    .Where(x => IsEnemyNear(x.To.Position, board, teamId) == false))
                {
                    //int currentDistance = Distance(ship.Position, move.Pirate.Position);
                    int newDistance = Distance(ship.Position, move.To.Position);
                    list.Add(new Tuple<int, Move>(newDistance, move));
                }
                if (list.Count > 0)
                {
                    int minDistance = list.Min(x => x.Item1);
                    goodMoves = list.Where(x => x.Item1 == minDistance).Select(x => x.Item2).ToList();
                }
            }
            if (goodMoves.Count == 0) //óíè÷òîæàåì âðàãà, åñëè îí ðÿäîì
            {
                foreach (Move move in availableMoves)
                {
                    if (IsEnemyPosition(move.To.Position, board, teamId)) goodMoves.Add(move);
                }
            }
            if (goodMoves.Count == 0)
            {
                List<Tile> tilesWithGold = new List<Tile>();
                //åñëè íà êàðòå åñòü îòêðûòîå çîëîòî, òî èäåì ê íåìó
                foreach (var tile in board.AllTiles(x => x.Type != TileType.Water && x.Coins > 0))
                {
                    tilesWithGold.Add(tile);
                }
                if (tilesWithGold.Count > 0) //íà êàðòå åñòü çîëîòî
                {
                    List<Tuple<int, Move>> list = new List<Tuple<int, Move>>();

                    foreach (Move move in availableMoves
                        .Where(x => x.WithCoins == false)
                        .Where(x => IsEnemyNear(x.To.Position, board, teamId) == false))
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
                //åñëè íà êàðòå åñòü íåîòêðûòûé ó÷àñòîê çîëîòî, òî èäåì ê íåìó
                foreach (var tile in board.AllTiles(x => x.Type == TileType.Unknown))
                {
                    tilesWithUnknown.Add(tile);
                }
                if (tilesWithUnknown.Count > 0) //íà êàðòå åñòü íåîòêðûòûé ó÷àñòîê 
                {
                    List<Tuple<int, Move>> list = new List<Tuple<int, Move>>();

                    foreach (Move move in availableMoves
                        .Where(x => x.WithCoins == false)
                        .Where(x => IsEnemyNear(x.To.Position, board, teamId) == false))
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

            //âûáèðàåì äîñòóïíûé õîä
            if (goodMoves.Count == 0)
            {
                goodMoves.AddRange(availableMoves);
            }

            var resultMove = goodMoves[Rnd.Next(goodMoves.Count)];
            for (int i = 0; i < availableMoves.Length; i++)
            {
                if (availableMoves[i] == resultMove)
                    return (i, null);
            }
            return (0, null);
        }

        private bool IsEnemyNear(Position to, Board board, int ourTeamId)
        {
            if (board.Map[to].Type == TileType.Water) return false;

            List<int> enemyList = board.Teams[ourTeamId].Enemies.ToList();
            for (int deltaX = -1; deltaX <= 1; deltaX++)
            {
                for (int deltaY = -1; deltaY <= 1; deltaY++)
                {
                    if (deltaX == 0 && deltaY == 0) continue;

                    var target = new Position(to.X + deltaX, to.Y + deltaY);

                    var occupationTeamId = board.Map[target].OccupationTeamId;
                    if (occupationTeamId.HasValue && enemyList.Exists(x => x == occupationTeamId.Value)) return true;
                }
            }
            return false;
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