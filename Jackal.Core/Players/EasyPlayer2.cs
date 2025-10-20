using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Domain;

namespace Jackal.Core.Players;

/// <summary>
/// Игрок простой бот - выбирает ход алгоритмом бей-неси,
/// рассчет дистанции упрощен через манхэттенское расстояние
/// </summary>
public class EasyPlayer2 : IPlayer
{
    private Random _rnd = new();
    
    public class BFSNode
    {
        public TilePosition CurrentPosition { get; set; }
        public List<AvailableMove> Path { get; set; } // Путь от стартовой позиции до текущей (может быть списком AvailableMove)
        public int Depth { get; set; }
        public SubTurnState CurrentSubTurnState { get; set; } // Состояние SubTurnState для текущего узла
        public TilePosition PreviousPositionInPath { get; set; } // Откуда пришли в CurrentPosition (для корректного расчета delta)
    }
    
    public SubTurnState DeepCloneSubTurnState(SubTurnState original)
    {
        return new SubTurnState
        {
            AirplaneFlying = original.AirplaneFlying,
            LighthouseViewCount = original.LighthouseViewCount,
            FallingInTheHole = original.FallingInTheHole,
            QuakePhase = original.QuakePhase,
            DrinkRumBottle = original.DrinkRumBottle,
            CannabisTurnCount = original.CannabisTurnCount
        };
    }
    
    public List<List<AvailableMove>> FindRoutesBFS(
        GameState gameState,
    AvailableMovesTask task,
    TilePosition startPosition,
    SubTurnState initialSubTurn,
    int maxDepth)
{
    // Очередь для BFS
    Queue<BFSNode> queue = new Queue<BFSNode>();
    
    // Давайте лучше будем сохранять visited-состояние для каждого узла
    HashSet<(TilePosition Position, TilePosition PreviousPosition, int Depth, SubTurnState State)> visited =
        new HashSet<(TilePosition, TilePosition, int, SubTurnState)>();

    // Список для хранения всех найденных маршрутов
    List<List<AvailableMove>> allRoutes = new List<List<AvailableMove>>();

    // Исходный узел
    BFSNode initialNode = new BFSNode
    {
        CurrentPosition = startPosition,
        Path = new List<AvailableMove>(), // Для начала путь пуст
        Depth = 0,
        CurrentSubTurnState = initialSubTurn,
        PreviousPositionInPath = startPosition // Или null, в зависимости от логики
    };

    queue.Enqueue(initialNode);
    // Добавляем начальное состояние в visited
    visited.Add((startPosition, startPosition, 0, initialSubTurn)); // Предполагаем, что SubTurnState имеет адекватные Equals/GetHashCode

    while (queue.Count > 0)
    {
        BFSNode currentNode = queue.Dequeue();

        if (currentNode.Depth >= maxDepth)
        {
            // Достигли максимальной глубины, этот путь завершен
            if (currentNode.Path.Any()) // Если путь не пустой (т.е. был сделан хотя бы один ход)
            {
                allRoutes.Add(currentNode.Path);
            }
            continue; // Не продолжаем поиск по этому пути
        }

        // Создадим временный `AvailableMovesTask` для `GetAllAvailableMoves` чтобы избежать модификации основного.
        var tempTask = new AvailableMovesTask(task.TeamId, task.Source, task.Prev)
        {
            AlreadyCheckedList = new List<CheckedPosition>(currentNode.Path
                .Select(move => new CheckedPosition(move.To, Position.GetDelta(move.Prev, move.To.Position)))) // Или другая логика для AlreadyCheckedList
        };

        // Список непосредственных шагов, доступных из currentNode.CurrentPosition
        List<AvailableMove> nextPossibleMoves = gameState.Board.GetAllAvailableMoves(
            tempTask,
            currentNode.CurrentPosition,
            currentNode.PreviousPositionInPath,
            currentNode.CurrentSubTurnState
        );

        foreach (var move in nextPossibleMoves)
        {
            // Здесь мы должны обновить SubTurnState, если ход влияет на него.
            // Например, уменьшить `LighthouseViewCount` или `Fuel`.
            SubTurnState nextSubTurnState = DeepCloneSubTurnState(currentNode.CurrentSubTurnState); // Создаем копию!
            // TODO: Применить изменения SubTurnState в зависимости от `move` (например, для Lighthouse)

            TilePosition nextPosition = move.To;

            // Проверяем, не посещали ли мы уже это состояние
            var newState = (nextPosition, currentNode.CurrentPosition, currentNode.Depth + 1, nextSubTurnState);
            if (visited.Contains(newState))
            {
                continue; // Уже посетили это состояние по этому или более короткому пути
            }

            // Создаем новый путь
            List<AvailableMove> newPath = new List<AvailableMove>(currentNode.Path) { move };

            BFSNode nextNode = new BFSNode
            {
                CurrentPosition = nextPosition,
                Path = newPath,
                Depth = currentNode.Depth + 1,
                CurrentSubTurnState = nextSubTurnState,
                PreviousPositionInPath = currentNode.CurrentPosition
            };


            queue.Enqueue(nextNode);
            visited.Add(newState); // Добавляем новое состояние в посещенные
        }
    }

    return allRoutes;
}
    
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
            
        var respawnPositions = board
            .AllTiles(x => x.Type == TileType.RespawnFort)
            .Select(x => x.Position)
            .ToList();
        
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
        goodMoves = gameState.AvailableMoves.Where(move => move.WithCoin && TargetIsShip(board, teamId, move)).ToList();
        if (CheckGoodMove(goodMoves, gameState.AvailableMoves, out goodMoveNum))
            return (goodMoveNum, null);
            
        // не ходим по чужим кораблям, людоедам и держим бабу
        Move[] safeAvailableMoves = gameState.AvailableMoves
            .Where(x => x.To != x.From)
            .Where(x => !enemyShipPositions.Contains(x.To.Position))
            .Where(x => !cannibalPositions.Contains(x.To.Position))
            .Where(x => !respawnPositions.Contains(x.From.Position))
            .ToArray();
            
        bool hasMoveWithCoins = safeAvailableMoves.Any(m => m.WithCoin || m.WithBigCoin);
        if (hasMoveWithCoins)
        {
            // перемещаем золото ближе к кораблю
            var escapePositions = board.AllTiles(x => x.Type == TileType.Balloon)
                .Select(x => x.Position)
                .ToList();
                
            escapePositions.Add(shipPosition);
                
            List<Tuple<int, Move>> list = [];
            foreach (Move move in safeAvailableMoves
                         .Where(x => x.WithCoin || x.WithBigCoin)
                         .Where(x => !waterPositions.Contains(x.To.Position))
                         .Where(x => IsEnemyNear(x.To.Position, board, teamId) == false))
            {
                // идем к самому ближнему выходу
                var minDistance = escapePositions
                    .Select(p => Distance(p, move.To.Position) + move.To.Level)
                    .Min();
                    
                var escapePosition = escapePositions
                    .First(p => Distance(p, move.To.Position) + move.To.Level == minDistance);
                    
                int currentDistance = Distance(escapePosition, move.From.Position) + move.From.Level;
                minDistance = Distance(escapePosition, move.To.Position) + move.To.Level;

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

        if (goodMoves.Count == 0)
        {
            // уничтожаем врага, если он рядом
            goodMoves = gameState.AvailableMoves.Where(move => IsEnemyPosition(move.To.Position, board, teamId)).ToList();
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
                         .Where(x => IsEnemyNear(x.To.Position, board, teamId) == false))
            {

                var minDistance = goldPositions
                    .Select(p => Distance(p, move.To.Position) + move.To.Level)
                    .Min();
                    
                var goldPosition = goldPositions
                    .First(p => Distance(p, move.To.Position) + move.To.Level == minDistance);
                    
                minDistance = Distance(goldPosition, move.To.Position) + move.To.Level;
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
                         .Where(x => IsEnemyNear(x.To.Position, board, teamId) == false))
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

    private static bool IsEnemyNear(Position to, Board board, int ourTeamId)
    {
        if (board.Map[to].Type == TileType.Water) return false;

        List<int> enemyList = board.Teams[ourTeamId].EnemyTeamIds.ToList();
        for (int deltaX = -1; deltaX <= 1; deltaX++)
        {
            for (int deltaY = -1; deltaY <= 1; deltaY++)
            {
                if (deltaX == 0 && deltaY == 0) continue;

                var target = new Position(to.X + deltaX, to.Y + deltaY);

                var occupationTeamId = board.Map[target].OccupationTeamId;
                if (occupationTeamId.HasValue && enemyList.Exists(x => x == occupationTeamId.Value)) 
                    return true;
            }
        }
            
        return false;
    }

    private static bool IsEnemyPosition(Position to, Board board, int teamId)
    {
        var occupationTeamId = board.Map[to].OccupationTeamId;
        if (occupationTeamId.HasValue &&
            board.Teams[teamId].EnemyTeamIds.ToList().Exists(x => x == occupationTeamId.Value) &&
            to != board.Teams[occupationTeamId.Value].ShipPosition)
        {
            return true;
        }
            
        return false;
    }

    private static bool TargetIsShip(Board board, int teamId, Move move)
    {
        var shipPosition = board.Teams[teamId].ShipPosition;
        return shipPosition == move.To.Position;
    }
        
    private static int MinDistance(List<Position> positions, Position to)
    {
        return positions.ConvertAll(x => Distance(x, to)).Min();
    }
        
    private static int Distance(Position pos1, Position pos2)
    {
        int deltaX = Math.Abs(pos1.X - pos2.X);
        int deltaY = Math.Abs(pos1.Y - pos2.Y);
        return Math.Max(deltaX, deltaY);
    }
        
    private static int WaterDistance(Position pos1, Position pos2)
    {
        int deltaX = Math.Abs(pos1.X - pos2.X);
        int deltaY = Math.Abs(pos1.Y - pos2.Y);
        return deltaX + deltaY;
    }
}