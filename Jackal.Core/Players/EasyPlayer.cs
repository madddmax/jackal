using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Domain;

namespace Jackal.Core.Players;

// todo 1 - разлом, меняем самую хорошую клетку на берегу противника на самую плохую из карты
// todo 2 - посмотреть почему кораблю не двигает в поисках лучшей высадки
// todo 3 - проверить поход по вертушкам

/// <summary>
/// Игрок простой бот - выбирает ход алгоритмом бей-неси,
/// рассчет дистанции упрощен через манхэттенское расстояние
/// </summary>
public class EasyPlayer : IPlayer
{
    private const int MaxDepth = 12;
    private const int TerminateDepth = int.MaxValue;
    private Random _rnd = new();

    private int _teamId;
    private Board _board = null!;
    private Dictionary<TilePosition, Dictionary<TilePosition, int>> _bfsRoutesFrom = null!;

    private List<Position> _holePositions = new();
    private List<Position> _openHolePositions = new();
    
    public void OnNewGame()
    {
        _rnd = new Random(1);
    }

    public (int moveNum, Guid? pirateId) OnMove(GameState gameState)
    {
        _teamId = gameState.TeamId;
        _board = gameState.Board;
        var shipPosition = _board.Teams[_teamId].ShipPosition; // todo учитывать союзный корабль
        
        _bfsRoutesFrom = new Dictionary<TilePosition, Dictionary<TilePosition, int>>();
        foreach (var move in gameState.AvailableMoves)
        {
            var newShipPosition = shipPosition;
            if (_board.Map[move.To.Position].Type == TileType.Water
                && move.From.Position == shipPosition
                && Board.Distance(move.From.Position, move.To.Position) == 1
                && Board.GetPossibleShipMoves(shipPosition, _board.MapSize).Contains(move.To.Position))
            {
                newShipPosition = move.To.Position;
            }

            CalcBfsRouteFrom(move.To, newShipPosition, MaxDepth);
        }
        
        var enemyTeamIds = _board.Teams[_teamId].EnemyTeamIds;
        
        var enemyShipPositions = _board.Teams
            .Where(t => enemyTeamIds.Contains(t.Id))
            .Select(t => t.ShipPosition)
            .ToList();
        
        var enemyPiratePositions = _board.AllPirates
            .Where(p => enemyTeamIds.Contains(p.TeamId))
            .Select(p => p.Position.Position)
            .Distinct()
            .ToList();
        
        var unknownPositions = _board
            .AllTiles(x => x.Type == TileType.Unknown)
            .Select(x => x.Position)
            .ToList();

        var waterPositions = _board
            .AllTiles(x => x.Type == TileType.Water)
            .Select(x => x.Position)
            .Except(new[] { shipPosition })
            .ToList();
            
        var escapePositions = _board.AllTiles(x => x.Type == TileType.Balloon)
            .Select(x => x.Position)
            .ToList();
                
        escapePositions.Add(shipPosition);
        
        // todo проблема забрать золото из клетки вертушки
        var goldPositions = _board
            .AllTiles(x => x.Type != TileType.Water && (x.Coins > 0 || x.BigCoins > 0))
            .Select(x => x.Position)
            .ToList();

        var takenGoldPosition = new List<Position>();
        foreach (var goldPosition in goldPositions)
        {
            var goldTilePosition = new TilePosition(goldPosition);
            CalcBfsRouteFrom(goldTilePosition, shipPosition, MaxDepth);

            foreach (var escapePosition in escapePositions)
            {
                var distance = Distance(goldTilePosition, new TilePosition(escapePosition));

                if (distance == TerminateDepth)
                    continue;
                
                takenGoldPosition.Add(goldPosition);
                break;
            }
        }
        
        var cannibalPositions = _board
            .AllTiles(x => x.Type == TileType.Cannibal)
            .Select(x => x.Position)
            .ToList();
            
        var trapPositions = _board
            .AllTiles(x => x.Type == TileType.Trap)
            .Select(x => x.Position)
            .ToList();
        
        _holePositions = _board
            .AllTiles(x => x.Type == TileType.Hole)
            .Select(x => x.Position)
            .ToList();
        
        var onlyOneHolePosition = _holePositions.Count > 1 ? new List<Position>() : _holePositions;

        _openHolePositions = _holePositions.Count > 1
            ? _holePositions
                .Where(p => !enemyPiratePositions.Contains(p))
                .ToList()
            : new List<Position>();
        
        var cannonPositions = _board
            .AllTiles(x => x.Type == TileType.Cannon)
            .Select(x => x.Position)
            .ToList();
            
        var respawnPositions = _board
            .AllTiles(x => x.Type == TileType.RespawnFort)
            .Select(x => x.Position)
            .ToList();
        
        // разыгрываем траву
        // ИД игрока команды за которую ходят не равна ИД игрока который ходит
        if (_board.Teams[_teamId].UserId != gameState.UserId)
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

            var piratesPosition = _board.Teams[_teamId].Pirates.Select(p => p.Position.Position);
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
                         .Where(x => takenGoldPosition.Contains(x.From.Position))
                         .Where(x => !waterPositions.Contains(x.To.Position)))
            {
                // идем к самому ближнему выходу
                var minDistance = escapePositions
                    .Select(p => Distance(move.To, new TilePosition(p)))
                    .Min();
                
                if(minDistance == TerminateDepth)
                    continue;
                
                list.Add(new Tuple<int, Move>(minDistance, move));
            }

            if (list.Count > 0)
            {
                var minDistance = list.Min(x => x.Item1);
                goodMoves = list.Where(x => x.Item1 == minDistance)
                    .Select(x => x.Item2)
                    .ToList();
            }
        }

        // не ходим на корабль и по шарам без монеты // todo можно отдельно обработать шар
        safeAvailableMoves = safeAvailableMoves
            .Where(x => !escapePositions.Contains(x.To.Position))
            .ToArray();
        
        if (goodMoves.Count == 0)
        {
            // уничтожаем врага, если он рядом
            goodMoves = gameState.AvailableMoves.Where(move => IsEnemyPositionAttack(move, _board, _teamId)).ToList();
            if (CheckGoodMove(goodMoves, gameState.AvailableMoves, out goodMoveNum))
                return (goodMoveNum, null);
        }
        
        if (goodMoves.Count == 0 && takenGoldPosition.Count > 0 && !hasMoveWithCoins)
        {
            // идем к самому ближнему золоту
            goodMoves = safeAvailableMoves
                .Where(x => x is { WithCoin: false, WithBigCoin: false })
                .Where(m => takenGoldPosition.Contains(m.To.Position))
                .ToList();
                
            if (CheckGoodMove(goodMoves, gameState.AvailableMoves, out goodMoveNum)) 
                return (goodMoveNum, null);
                
            List<Tuple<int, Move>> list = [];
            foreach (Move move in safeAvailableMoves
                         .Where(x => x is { WithCoin: false, WithBigCoin: false })
                         .Where(x => !waterPositions.Contains(x.From.Position)))
            {
                var minDistance = takenGoldPosition
                    .Select(p => Distance(move.To, new TilePosition(p)))
                    .Min();
                    
                if(minDistance == TerminateDepth)
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

        if (goodMoves.Count == 0 && unknownPositions.Count != 0)
        {
            // идем к самой ближней неизвестной клетке
            List<Tuple<int, Move>> list = new List<Tuple<int, Move>>();
            foreach (Move move in safeAvailableMoves
                         .Where(x => x is { WithCoin: false, WithBigCoin: false })
                         .Where(x => !waterPositions.Contains(x.From.Position)))
            {
                var minDistance = unknownPositions
                    .Select(p => Distance(move.To, new TilePosition(p)))
                    .Min();
                
                if(minDistance == TerminateDepth)
                    continue;
                
                list.Add(new Tuple<int, Move>(minDistance, move));
            }

            if (list.Count > 0)
            {
                var minDistance = list.Min(x => x.Item1);
                goodMoves = list.Where(x => x.Item1 == minDistance)
                    .Select(x => x.Item2)
                    .ToList();
            }
        }

        if (goodMoves.Count == 0)
        {
            // залазим на свой корабль или бьемся об чужой
            List<Tuple<int, Move>> list = [];
            foreach (Move move in gameState.AvailableMoves.Where(x => waterPositions.Contains(x.From.Position)))
            {
                int distance = WaterDistance(move.To.Position, shipPosition);
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
    /// Можем ударить врага
    /// </summary>
    private static bool IsEnemyPositionAttack(Move move, Board board, int teamId)
    {
        if (board.Map[move.To.Position].Type == TileType.Jungle)
        {
            return false;
        }
        
        var enemyTeamIds = board.Teams[teamId].EnemyTeamIds;
        foreach (var enemyTeamId in enemyTeamIds)
        {
            var enemyTeam = board.Teams[enemyTeamId];
            
            // не атакуем вражеский корабль
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

    private void CalcBfsRouteFrom(TilePosition position, Position shipPosition, int maxDepth)
    {
        if (_bfsRoutesFrom.ContainsKey(position))
        {
            return;
        }
        
        _bfsRoutesFrom[position] = new Dictionary<TilePosition, int>();

        var queue = new Queue<BfsNode>();
        queue.Enqueue(new BfsNode(position, 1));
        
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            if (currentNode.Depth == maxDepth)
            {
                continue;
            }
            
            var depth = currentNode.Depth + 1;
            var nextPossibleMoves = GetAvailableMoves(
                currentNode.Position, _teamId, shipPosition
            );
            
            foreach (var move in nextPossibleMoves)
            {
                if(_board.Map[move.To.Position].Type == TileType.Hole 
                   && _holePositions.Count > 1
                   && _openHolePositions.Count > 0) // todo рассмотреть случай если одна открытая позиция и мы туда идем
                {
                    foreach (var openHolePosition in _openHolePositions)
                    {
                        var moveToNextHolePosition = new TilePosition(openHolePosition);
                        
                        if(_bfsRoutesFrom[position].ContainsKey(moveToNextHolePosition))
                        {
                            // ранее был найден путь короче
                            break;
                        }
                        
                        _bfsRoutesFrom[position].Add(moveToNextHolePosition, depth);
                        var nextNode = new BfsNode(moveToNextHolePosition, depth);
                        queue.Enqueue(nextNode);
                    }
                }
                else
                {
                    if(_bfsRoutesFrom[position].ContainsKey(move.To))
                    {
                        continue;
                    }

                    if (_board.Map[move.To.Position].Type == TileType.Water
                        && move.To.Position != shipPosition)
                    {
                        continue;
                    }
                    
                    _bfsRoutesFrom[position].Add(move.To, depth);

                    if (_board.Map[move.To.Position].Type != TileType.Unknown
                        && _board.Map[move.To.Position].Type != TileType.Balloon
                        && _board.Map[move.To.Position].Type != TileType.Cannibal
                        && _board.Map[move.To.Position].Type != TileType.Crocodile
                        && _board.Map[move.To.Position].Type != TileType.Cannon
                        && _board.Map[move.To.Position].Type != TileType.Fort
                        && _board.Map[move.To.Position].Type != TileType.RespawnFort
                        && _board.Map[move.To.Position].Type != TileType.Jungle)
                    {
                        var nextNode = new BfsNode(move.To, depth);
                        queue.Enqueue(nextNode);
                    }
                }
            }
        }
    }
    
    private int Distance(TilePosition from, TilePosition to)
    {
        if (from == to)
        {
            return 1;
        }

        if (_bfsRoutesFrom.TryGetValue(from, out var routesTo) &&
            routesTo.TryGetValue(to, out var distance))
        {
            return distance;
        }

        return TerminateDepth;
    }

    private List<AvailableMove> GetAvailableMoves(TilePosition position, int teamId, Position shipPosition)
    {
        var team = _board.Teams[teamId];
        var task = new AvailableMovesTask(team.Id, position, position);
        var subTurnState = new SubTurnState(); // todo нет состояния хождения в дыру
        return _board.GetAllAvailableMoves(
            task,
            task.Source,
            task.Prev,
            subTurnState,
            [shipPosition]
        );
    }
        
    private static int WaterDistance(Position pos1, Position pos2)
    {
        int deltaX = Math.Abs(pos1.X - pos2.X);
        int deltaY = Math.Abs(pos1.Y - pos2.Y);
        return deltaX + deltaY;
    }
}

public record BfsNode(TilePosition Position, int Depth);