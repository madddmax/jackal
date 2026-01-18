using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Domain;

namespace Jackal.Core.Players;

/// <summary>
/// Игрок простой бот - выбирает ход алгоритмом бей-неси,
/// рассчет дистанции упрощен через манхэттенское расстояние
/// </summary>
public class VeryEasyPlayer : IPlayer
{
    private Random _rnd = new();
    
    public void OnNewGame()
    {
        _rnd = new Random(1);
    }

    public (int moveNum, Guid? pirateId) OnMove(GameState gameState)
    {
        int teamId = gameState.TeamId;
        Board board = gameState.Board;
        var shipPosition = board.Teams[teamId].ShipPosition;

        var enemyShipPositions = board.Teams
            .Select(t => t.ShipPosition)
            .Where(p => p != shipPosition)
            .ToList();
            
        var unknownPositions = board
            .AllTiles(x => x.Type == TileType.Unknown)
            .Select(x => x.Position)
            .ToList();

        var waterPositions = board
            .AllTiles(x => x.Type == TileType.Water)
            .Select(x => x.Position)
            .Except(new[] { shipPosition })
            .ToList();
            
        var goldPositions = board
            .AllTiles(x => x.Type != TileType.Water && (x.Coins > 0 || x.BigCoins > 0))
            .Select(x => x.Position)
            .ToList();
            
        var cannibalPositions = board
            .AllTiles(x => x.Type == TileType.Cannibal)
            .Select(x => x.Position)
            .ToList();
            
        var trapPositions = board
            .AllTiles(x => x.Type == TileType.Trap)
            .Select(x => x.Position)
            .ToList();
        
        var holePositions = board
            .AllTiles(x => x.Type == TileType.Hole)
            .Select(x => x.Position)
            .ToList();
        
        var onlyOneHolePosition = holePositions.Count > 1 ? new List<Position>() : holePositions;
        
        var cannonPositions = board
            .AllTiles(x => x.Type == TileType.Cannon)
            .Select(x => x.Position)
            .ToList();
            
        var respawnPositions = board
            .AllTiles(x => x.Type == TileType.RespawnFort)
            .Select(x => x.Position)
            .ToList();
        
        var escapePositions = board.AllTiles(x => x.Type == TileType.Balloon)
            .Select(x => x.Position)
            .ToList();
                
        escapePositions.Add(shipPosition);

        // разыгрываем траву
        // ИД игрока команды за которую ходят не равна ИД игрока который ходит
        if (board.Teams[teamId].UserId != gameState.UserId)
        {
            // идем к людоеду
            var cannibalMoves = gameState.AvailableMoves
                .Where(x => cannibalPositions.Contains(x.To.Position))
                .ToList();
            
            if (CheckGoodMove(cannibalMoves, gameState.AvailableMoves, out var badMoveNum))
                return (badMoveNum, null);
            
            // бьемся об чужой корабль
            var enemyShipMoves = gameState.AvailableMoves
                .Where(x => enemyShipPositions.Contains(x.To.Position))
                .ToList();
            
            if (CheckGoodMove(enemyShipMoves, gameState.AvailableMoves, out badMoveNum))
                return (badMoveNum, null);   
            
            // топим монету
            var waterMoves = gameState.AvailableMoves
                .Where(x => x.WithCoin || x.WithBigCoin)
                .Where(x => waterPositions.Contains(x.To.Position))
                .ToList();
            
            if (CheckGoodMove(waterMoves, gameState.AvailableMoves, out badMoveNum))
                return (badMoveNum, null);
            
            // уходим с воскрешения
            var fromRespawnMoves = gameState.AvailableMoves
                .Where(x => respawnPositions.Contains(x.From.Position))
                .Where(x => !respawnPositions.Contains(x.To.Position))
                .ToList();
            
            if (CheckGoodMove(fromRespawnMoves, gameState.AvailableMoves, out badMoveNum))
                return (badMoveNum, null);
            
            return (_rnd.Next(gameState.AvailableMoves.Length), null);
        }
        
        // воскрешаемся если можем
        List<Move> goodMoves = gameState.AvailableMoves.Where(m => m.Type == MoveType.WithRespawn).ToList();
        if (CheckGoodMove(goodMoves, gameState.AvailableMoves, out var goodMoveNum))
            return (goodMoveNum, null);
        
        // освобождаем пирата из ловушек
        foreach (var trapPosition in trapPositions)
        {
            if (gameState.AvailableMoves.Any(m => m.From.Position == trapPosition))
            {
                continue;
            }

            var piratesPosition = board.Teams[teamId].Pirates.Select(p => p.Position.Position);
            goodMoves = gameState.AvailableMoves
                .Where(m => trapPosition == m.To.Position && piratesPosition.Contains(trapPosition))
                .ToList();
                
            if (CheckGoodMove(goodMoves, gameState.AvailableMoves, out goodMoveNum))
                return (goodMoveNum, null);
        }

        // заносим золото на корабль
        goodMoves = gameState.AvailableMoves.Where(move => move.WithCoin && escapePositions.Contains(move.To.Position)).ToList();
        if (CheckGoodMove(goodMoves, gameState.AvailableMoves, out goodMoveNum))
            return (goodMoveNum, null);
            
        // не ходим туда-обратно,
        // не ходим по чужим кораблям,
        // не ходим по людоедам,
        // не ходим по не открытым дырам,
        // не ходим по пушкам,
        // держим бабу
        Move[] safeAvailableMoves = gameState.AvailableMoves
            .Where(x => x.To != x.From)
            .Where(x => !enemyShipPositions.Contains(x.To.Position))
            .Where(x => !cannibalPositions.Contains(x.To.Position))
            .Where(x => !onlyOneHolePosition.Contains(x.To.Position))
            .Where(x => !cannonPositions.Contains(x.To.Position))
            .Where(x => !respawnPositions.Contains(x.From.Position))
            .ToArray();
            
        bool hasMoveWithCoins = safeAvailableMoves.Any(m => m.WithCoin || m.WithBigCoin);
        if (hasMoveWithCoins)
        {
            // перемещаем золото ближе к кораблю
            List<Tuple<int, Move>> list = [];
            foreach (Move move in safeAvailableMoves
                         .Where(x => x.WithCoin || x.WithBigCoin)
                         .Where(x => !waterPositions.Contains(x.To.Position))
                         .Where(x => IsEnemyNearDefense(x, board, teamId) == false))
            {
                // идем к самому ближнему выходу
                var minDistance = escapePositions
                    .Select(p => Board.Distance(p, move.To.Position) + move.To.Level)
                    .Min();
                    
                var escapePosition = escapePositions
                    .First(p => Board.Distance(p, move.To.Position) + move.To.Level == minDistance);
                    
                int currentDistance = Board.Distance(escapePosition, move.From.Position) + move.From.Level;
                minDistance = Board.Distance(escapePosition, move.To.Position) + move.To.Level;

                if (currentDistance <= minDistance)
                    continue;

                list.Add(new Tuple<int, Move>(minDistance, move));
            }

            if (list.Count > 0)
            {
                int minDistance = list.Min(x => x.Item1);
                goodMoves = list.Where(x => x.Item1 == minDistance)
                    .Select(x => x.Item2)
                    .ToList();
            }
        }

        // не ходим на корабль и по шарам без монеты
        safeAvailableMoves = safeAvailableMoves
            .Where(x => !escapePositions.Contains(x.To.Position))
            .ToArray();
        
        if (goodMoves.Count == 0)
        {
            // уничтожаем врага, если он рядом
            goodMoves = gameState.AvailableMoves.Where(move => IsEnemyPositionAttack(move, board, teamId)).ToList();
            if (CheckGoodMove(goodMoves, gameState.AvailableMoves, out goodMoveNum))
                return (goodMoveNum, null);
        }
        
        if (goodMoves.Count == 0 && goldPositions.Count > 0 && !hasMoveWithCoins)
        {
            // идем к самому ближнему золоту
            goodMoves = safeAvailableMoves
                .Where(x => x is { WithCoin: false, WithBigCoin: false })
                .Where(m => goldPositions.Contains(m.To.Position))
                .ToList();
                
            if (CheckGoodMove(goodMoves, gameState.AvailableMoves, out goodMoveNum)) 
                return (goodMoveNum, null);
                
            List<Tuple<int, Move>> list = [];
            foreach (Move move in safeAvailableMoves
                         .Where(x => x.WithCoin == false)
                         .Where(x => !waterPositions.Contains(x.From.Position))
                         .Where(x => IsEnemyNearDefense(x, board, teamId) == false))
            {

                var minDistance = goldPositions
                    .Select(p => Board.Distance(p, move.To.Position) + move.To.Level)
                    .Min();
                    
                var goldPosition = goldPositions
                    .First(p => Board.Distance(p, move.To.Position) + move.To.Level == minDistance);
                    
                minDistance = Board.Distance(goldPosition, move.To.Position) + move.To.Level;
                list.Add(new Tuple<int, Move>(minDistance, move));
            }

            if (list.Count > 0)
            {
                int minDistance = list.Min(x => x.Item1);
                goodMoves = list.Where(x => x.Item1 == minDistance)
                    .Select(x => x.Item2)
                    .ToList();
            }
        }

        if (goodMoves.Count == 0 && unknownPositions.Count != 0)
        {
            // идем к самой ближней неизвестной клетке
            List<Tuple<int, Move>> list = new List<Tuple<int, Move>>();
            foreach (Move move in safeAvailableMoves
                         .Where(x => x.WithCoin == false)
                         .Where(x => !waterPositions.Contains(x.From.Position))
                         .Where(x => IsEnemyNearDefense(x, board, teamId) == false))
            {
                var minDistance = MinDistance(unknownPositions, move.To.Position) + move.To.Level;
                list.Add(new Tuple<int, Move>(minDistance, move));
            }

            if (list.Count > 0)
            {
                var minDistance = list.Min(x => x.Item1);
                goodMoves = list.Where(x => x.Item1 == minDistance).Select(x => x.Item2).ToList();
            }
        }

        if (goodMoves.Count == 0)
        {
            // залазим на свой корабль или бьемся об чужой
            List<Tuple<int, Move>> list = [];
            foreach (Move move in gameState.AvailableMoves.Where(x => waterPositions.Contains(x.From.Position)))
            {
                int distance = WaterDistance(shipPosition, move.To.Position);
                list.Add(new Tuple<int, Move>(distance, move));
            }

            if (list.Count > 0)
            {
                int minDistance = list.Min(x => x.Item1);
                goodMoves = list.Where(x => x.Item1 == minDistance)
                    .Select(x => x.Item2)
                    .ToList();
            }
        }

        if (goodMoves.Count == 0)
        {
            // выбираем любой доступный ход
            goodMoves.AddRange(gameState.AvailableMoves);
        }

        if (CheckGoodMove(goodMoves, gameState.AvailableMoves, out goodMoveNum)) 
            return (goodMoveNum, null);
            
        return (0, null);
    }

    private bool CheckGoodMove(List<Move> moves, Move[] availableMoves, out int moveNum)
    {
        moveNum = 0;
        if (moves.Count == 0)
            return false;
            
        var resultMove = moves[_rnd.Next(moves.Count)];
        for (int i = 0; i < availableMoves.Length; i++)
        {
            if (availableMoves[i] == resultMove)
            {
                moveNum = i;
                return true;
            }
        }
            
        return false;
    }

    /// <summary>
    /// Враг в пределах одного хода
    /// </summary>
    private static bool IsEnemyNearDefense(Move move, Board board, int teamId)
    {
        // не боимся если это наша зона высадки
        var shipPosition = board.Teams[teamId].ShipPosition;
        if (move.From.Position == shipPosition)
        {
            return false;
        }
        
        // не боимся если плаваем
        var moveTo = move.To;
        if (board.Map[moveTo.Position].Type == TileType.Water)
        {
            return false;
        }

        var enemyTeamIds = board.Teams[teamId].EnemyTeamIds;
        foreach (var enemyTeamId in enemyTeamIds)
        {
            var enemyTeam = board.Teams[enemyTeamId];
            var enemyPirates = enemyTeam.Pirates.Where(x => x.IsActive);
            foreach (var enemyPirate in enemyPirates)
            {
                if (enemyPirate.Position == moveTo)
                {
                    return true;
                }
                
                var task = new AvailableMovesTask(enemyPirate.TeamId, enemyPirate.Position, enemyPirate.Position);
                var subTurnState = new SubTurnState { DrinkRumBottle = enemyTeam.RumBottles > 0 };
                var moves = board.GetAllAvailableMoves(
                    task,
                    task.Source,
                    task.Prev,
                    subTurnState,
                    [shipPosition]
                );

                if (moves.Any(m => m.To == moveTo))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Можем ударить врага
    /// </summary>
    private static bool IsEnemyPositionAttack(Move move, Board board, int teamId)
    {
        var enemyTeamIds = board.Teams[teamId].EnemyTeamIds;
        foreach (var enemyTeamId in enemyTeamIds)
        {
            var enemyTeam = board.Teams[enemyTeamId];
            
            // не атакуем вражескую зону высадки
            var enemyShipPosition = enemyTeam.ShipPosition;
            if (move.To.Position == enemyShipPosition)
            {
                return false;
            }
            
            var enemyPirates = enemyTeam.Pirates.Where(x => x.IsActive);
            if (enemyPirates.Any(enemyPirate => enemyPirate.Position == move.To))
            {
                return true;
            }
        }

        return false;
    }
        
    private static int MinDistance(List<Position> positions, Position to)
    {
        return positions.ConvertAll(x => Board.Distance(x, to)).Min();
    }
        
    private static int WaterDistance(Position pos1, Position pos2)
    {
        int deltaX = Math.Abs(pos1.X - pos2.X);
        int deltaY = Math.Abs(pos1.Y - pos2.Y);
        return deltaX + deltaY;
    }
}