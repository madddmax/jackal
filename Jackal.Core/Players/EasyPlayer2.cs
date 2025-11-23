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
    public class BFSNode
    {
        public TilePosition CurrentPosition { get; set; }
        public List<Move> Path { get; set; } // Путь от стартовой позиции до текущей (может быть списком AvailableMove)
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

    private List<Move> FindRoutesBfs(
        GameState gameState,
        TilePosition startPosition)
    {
        int teamId = gameState.TeamId;
        Board board = gameState.Board;
        
        AvailableMovesTask task = new AvailableMovesTask(teamId, startPosition, startPosition);
        var initialSubTurn = new SubTurnState();
        
        // Очередь для BFS
        Queue<BFSNode> queue = new Queue<BFSNode>();

        // Список для хранения всех найденных маршрутов
        var allRoutes = new List<Move>();

        // Исходный узел
        BFSNode initialNode = new BFSNode
        {
            CurrentPosition = startPosition,
            Path = new List<Move>(), // Для начала путь пуст
            Depth = 0,
            CurrentSubTurnState = initialSubTurn,
            PreviousPositionInPath = startPosition // Или null, в зависимости от логики
        };

        queue.Enqueue(initialNode);

        while (queue.Count > 0)
        {
            BFSNode currentNode = queue.Dequeue();

            if (currentNode.Depth >= MaxDepth)
            {
                // Достигли максимальной глубины, этот путь завершен
                if (currentNode.Path.Any()) // Если путь не пустой (т.е. был сделан хотя бы один ход)
                {
                    allRoutes.AddRange(currentNode.Path);
                }

                continue; // Не продолжаем поиск по этому пути
            }

            // Создадим временный `AvailableMovesTask` для `GetAllAvailableMoves` чтобы избежать модификации основного.
            var tempTask = new AvailableMovesTask(task.TeamId, task.Source, task.Prev)
            {
                AlreadyCheckedList = new List<CheckedPosition>(
                    currentNode.Path.Select(move => new CheckedPosition(move.To))
                )
            };

            // Список непосредственных шагов, доступных из currentNode.CurrentPosition
            List<AvailableMove> nextPossibleMoves = board.GetAllAvailableMoves(
                tempTask,
                currentNode.CurrentPosition,
                currentNode.PreviousPositionInPath,
                currentNode.CurrentSubTurnState
            );

            foreach (var move in nextPossibleMoves)
            {
                // Здесь мы должны обновить SubTurnState, если ход влияет на него.
                // Например, уменьшить `LighthouseViewCount` или `Fuel`.
                SubTurnState
                    nextSubTurnState = DeepCloneSubTurnState(currentNode.CurrentSubTurnState); // Создаем копию!
                // TODO: Применить изменения SubTurnState в зависимости от `move` (например, для Lighthouse)

                TilePosition nextPosition = move.To;

                // Проверяем, не посещали ли мы уже это состояние
                if (_bfsVisited.Contains(nextPosition))
                {
                    continue; // Уже посетили это состояние по этому или более короткому пути
                }

                // Создаем новый путь
                List<Move> newPath = new List<Move>(currentNode.Path) { move.ToMove };

                BFSNode nextNode = new BFSNode
                {
                    CurrentPosition = nextPosition,
                    Path = newPath,
                    Depth = currentNode.Depth + 1,
                    CurrentSubTurnState = nextSubTurnState,
                    PreviousPositionInPath = currentNode.CurrentPosition
                };

                queue.Enqueue(nextNode);
                _bfsVisited.Add(nextPosition); // Добавляем новое состояние в посещенные
            }
        }

        return allRoutes;
    }

    public void OnNewGame()
    {
    }

    private int _teamId;
    private Board _board;
    private Team _team;
    private Position _shipPosition;

    private Move[] _availableMoves;
    private HashSet<TilePosition> _bfsVisited;

    private List<Position> _escapePositions;
    private List<Position> _enemyShipPositions;
    private List<Position> _unknownPositions;
    private List<Position> _waterPositions;
    private List<Position> _coinPositions;
    private List<Position> _bigCoinPositions;
    private List<Position> _cannibalPositions;
    private List<Position> _trapPositions;
    private List<Position> _respawnPositions;
    
    public (int moveNum, Guid? pirateId) OnMove(GameState gameState)
    {
        _teamId = gameState.TeamId;
        _board = gameState.Board;
        _team = _board.Teams[_teamId];
        _shipPosition = _team.ShipPosition;

        _availableMoves = gameState.AvailableMoves;
        _bfsVisited = gameState.AvailableMoves.Select(m => m.To).ToHashSet();
        
        _escapePositions = _board.AllTiles(x => x.Type == TileType.Balloon)
            .Select(x => x.Position)
            .ToList();

        _escapePositions.Add(_shipPosition);
        
        _enemyShipPositions = _board.Teams
            .Select(t => t.ShipPosition)
            .Where(p => p != _shipPosition)
            .ToList();
            
        _unknownPositions = _board
            .AllTiles(x => x.Type == TileType.Unknown)
            .Select(x => x.Position)
            .ToList();

        _waterPositions = _board
            .AllTiles(x => x.Type == TileType.Water)
            .Select(x => x.Position)
            .Except(new[] { _shipPosition })
            .ToList();
            
        _coinPositions = _board
            .AllTiles(x => x.Type != TileType.Water && x.Coins > 0)
            .Select(x => x.Position)
            .ToList();
        
        _bigCoinPositions = _board
            .AllTiles(x => x.Type != TileType.Water && x.BigCoins > 0)
            .Select(x => x.Position)
            .ToList();
            
        _cannibalPositions = _board
            .AllTiles(x => x.Type == TileType.Cannibal)
            .Select(x => x.Position)
            .ToList();
            
        _trapPositions = _board
            .AllTiles(x => x.Type == TileType.Trap)
            .Select(x => x.Position)
            .ToList();
            
        _respawnPositions = _board
            .AllTiles(x => x.Type == TileType.RespawnFort)
            .Select(x => x.Position)
            .ToList();
        
        // Список для хранения всех найденных маршрутов
        var allRoutes = new Dictionary<Move, List<Move>>();
        foreach (var move in gameState.AvailableMoves)
        {
            var route = FindRoutesBfs(gameState, move.To);
            allRoutes.Add(move, route);
        }
        
        int bestScore = 0;
        Move? bestMove = null;
        
        foreach ((Move currentMove, List<Move> route) in allRoutes)
        {
            int currentScore = CalcScore(currentMove, route);
            if (currentScore > bestScore)
            {
                bestMove = currentMove;
                bestScore = currentScore;
            }
        }
        
        if (GetMove(bestMove, _availableMoves, out var moveNum)) 
            return (moveNum, null);
            
        return (0, null);
    }

    private const int MaxDepth = 2;
    private const int BigCoinScore = 120;
    private const int RespawnScore = 100;
    private const int TrapScore = 80;
    private const int TakeBigCoinScore = 60;
    private const int FightScore = 50;
    private const int CoinScore = 40;
    private const int TakeCoinScore = 20;
    private const int BackFromWaterScore = 10;
    private const int SpinningScore = 8;
    private const int OpenScore = 6;
    
    // todo надо объединить текущий ход в маршрут + указывать глубину по которой считать score
    // todo внести расчет и сравнение score в bfs
    private int CalcScore(Move currentMove, List<Move> route)
    {
        // не ходим туда-сюда и держим бабу
        if (currentMove.From == currentMove.To ||
            _respawnPositions.Contains(currentMove.From.Position))
        {
            return 0;
        }
        
        // не ходим по чужим кораблям и людоедам
        if (_enemyShipPositions.Contains(currentMove.To.Position) ||
            _cannibalPositions.Contains(currentMove.To.Position))
        {
            return 0;
        }

        if (_unknownPositions.Contains(currentMove.To.Position))
        {
            return OpenScore;
        }

        int score = 0;
        
        // воскрешаемся если можем
        if (currentMove.Type == MoveType.WithRespawn)
        {
            score += RespawnScore;
        }

        if (route.Any(m => m.Type == MoveType.WithRespawn))
        {
            score += RespawnScore / 2;
        }

        // освобождаем пирата из ловушек
        foreach (var trapPosition in _trapPositions)
        {
            if (_availableMoves.Any(m => m.From.Position == trapPosition))
            {
                continue;
            }

            var piratesPosition = _board.Teams[_teamId].Pirates
                .Select(p => p.Position.Position)
                .ToList();
            
            if (currentMove.To.Position == trapPosition && piratesPosition.Contains(trapPosition))
            {
                score += TrapScore;
            }

            if (route.Any(m => m.To.Position == trapPosition && piratesPosition.Contains(trapPosition)))
            {
                score += TrapScore / 2;
            }
        }

        // заносим золото на корабль
        if (currentMove.WithBigCoin && _escapePositions.Contains(currentMove.To.Position))
        {
            score += BigCoinScore;
        }
        
        if(route.Any(move => move.WithBigCoin && _escapePositions.Contains(move.To.Position)))
        {
            score += BigCoinScore / 2;
        }
        
        if (currentMove.WithCoin && _escapePositions.Contains(currentMove.To.Position))
        {
            score += CoinScore;
        }
        
        if(route.Any(move => move.WithCoin && _escapePositions.Contains(move.To.Position)))
        {
            score += CoinScore / 2;
        }
        
        // уничтожаем врага, если он рядом
        if (IsEnemyPosition(currentMove.To.Position, _board, _teamId))
        {
            score += FightScore;
        }

        if (route.Any(move => IsEnemyPosition(move.To.Position, _board, _teamId)))
        {
            score += FightScore / 2;
        }

        // идём к золоту
        if (currentMove is { WithCoin: false, WithBigCoin: false } && 
            _bigCoinPositions.Contains(currentMove.To.Position))
        {
            score += TakeBigCoinScore;
        }
        
        if(currentMove is { WithCoin: false, WithBigCoin: false } && 
           route.Any(move => move is { WithCoin: false, WithBigCoin: false } && _bigCoinPositions.Contains(move.To.Position)))
        {
            score += TakeBigCoinScore / 2;
        }
        
        if (currentMove is { WithCoin: false, WithBigCoin: false } && 
            _coinPositions.Contains(currentMove.To.Position))
        {
            score += TakeCoinScore;
        }
        
        if(currentMove is { WithCoin: false, WithBigCoin: false } && 
           route.Any(move => move is { WithCoin: false, WithBigCoin: false } && _coinPositions.Contains(move.To.Position)))
        {
            score += TakeCoinScore / 2;
        }

        // открываем новые клетки
        if (_unknownPositions.Contains(currentMove.To.Position))
        {
            score += OpenScore;
        }
        
        if(route.Any(move => _unknownPositions.Contains(move.To.Position)))
        {
            score += OpenScore / 2;
        }
        
        // залазим на свой корабль
        if (_waterPositions.Contains(currentMove.From.Position))
        {
            if (currentMove.To.Position == _shipPosition)
            {
                score += BackFromWaterScore;
            }

            if (route.Any(m => m.To.Position == _shipPosition))
            {
                score += BackFromWaterScore / 2;
            }
        }

        return score;
    }

    private static bool GetMove(Move? move, Move[] availableMoves, out int moveNum)
    {
        moveNum = 0;
        if (move == null)
            return false;
        
        for (int i = 0; i < availableMoves.Length; i++)
        {
            if (availableMoves[i] == move)
            {
                moveNum = i;
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
}