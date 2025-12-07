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
        public TilePosition ToPosition { get; set; }
        
        public TilePosition FromPosition { get; set; }
        
        public int Depth { get; set; }
        
        public int Score { get; set; }
        
        public List<Move> Path { get; set; } // Путь от стартовой позиции до текущей (может быть списком AvailableMove)
        
        public SubTurnState CurrentSubTurnState { get; set; } // Состояние SubTurnState для текущего узла
        
        /// <summary>
        /// Изначальный ход
        /// </summary>
        public Move? FirstMove { get; set; }
    }

    private static SubTurnState DeepCloneSubTurnState(SubTurnState original)
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

    private Move? FindRoutesBfs(GameState gameState)
    {
        int teamId = gameState.TeamId;
        Board board = gameState.Board;
        var availableMoves = gameState.AvailableMoves;

        int bestScore = 0;
        BFSNode bestNode = null;
        
        // Очередь для BFS
        Queue<BFSNode> queue = new Queue<BFSNode>();
        HashSet<TilePosition> bfsVisited = new HashSet<TilePosition>();
        
        foreach (var move in availableMoves)
        {
            // Исходный узел
            var initialNode = new BFSNode
            {
                ToPosition = move.To,
                FromPosition = move.From,
                Score = CalcScore(move, out var hasCoin),
                Path = [],
                Depth = 0,
                CurrentSubTurnState = new SubTurnState(),
                FirstMove = move
            };

            if (initialNode.Score > bestScore)
            {
                bestScore = initialNode.Score;
                bestNode = initialNode;
            }
            
            queue.Enqueue(initialNode);
            bfsVisited.Add(move.To);
        }
        
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            if (currentNode.Depth >= MaxDepth)
            {
                continue;
            }
            
            var availableMovesTask = new AvailableMovesTask(teamId, currentNode.ToPosition, currentNode.ToPosition)
            {
                AlreadyCheckedList = new List<CheckedPosition>(
                    currentNode.Path.Select(move => new CheckedPosition(move.To))
                )
            };
            
            var nextPossibleMoves = board.GetAllAvailableMoves(
                availableMovesTask,
                currentNode.ToPosition,
                currentNode.FromPosition,
                currentNode.CurrentSubTurnState
            );

            foreach (var move in nextPossibleMoves)
            {
                var nextPosition = move.To;
                if (bfsVisited.Contains(nextPosition))
                {
                    continue;
                }
                
                var nextSubTurnState = DeepCloneSubTurnState(currentNode.CurrentSubTurnState);
                int score = currentNode.Score + CalcScore(move.ToMove, out var hasCoin) / (currentNode.Depth + 1);
                
                // Создаем новый путь
                List<Move> newPath = new List<Move>(currentNode.Path) { move.ToMove };
                
                var nextNode = new BFSNode
                {
                    ToPosition = nextPosition,
                    FromPosition = move.From,
                    Path = newPath,
                    Score = score,
                    Depth = currentNode.Depth + 1,
                    CurrentSubTurnState = nextSubTurnState,
                    FirstMove = currentNode.FirstMove
                };

                if (nextNode.Score > bestScore)
                {
                    bestScore = nextNode.Score;
                    bestNode = nextNode;
                }
                
                queue.Enqueue(nextNode);
                bfsVisited.Add(nextPosition);
            }
        }

        return bestNode?.FirstMove;
    }

    public void OnNewGame()
    {
    }

    private int _teamId;
    private Board _board;
    private Team _team;
    private Position _shipPosition;

    private Move[] _availableMoves;

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
        
        var bestMove = FindRoutesBfs(gameState);
        
        if (GetMove(bestMove, _availableMoves, out var moveNum)) 
            return (moveNum, null);
            
        return (0, null);
    }

    private const int MaxDepth = 4;
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
    private int CalcScore(Move currentMove, out bool hasCoin)
    {
        // todo менять карту в зависимости от пред ходов
        hasCoin = false;
        
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
        }

        // заносим золото на корабль
        if (currentMove.WithBigCoin && _escapePositions.Contains(currentMove.To.Position))
        {
            score += BigCoinScore;
        }
        
        if (currentMove.WithCoin && _escapePositions.Contains(currentMove.To.Position))
        {
            score += CoinScore;
        }
        
        // тащим монету
        if (currentMove.WithBigCoin)
        {
            score += TakeBigCoinScore;
        }
        
        if (currentMove.WithCoin)
        {
            score += TakeCoinScore;
        }
        
        // уничтожаем врага, если он рядом
        if (IsEnemyPosition(currentMove.To.Position, _board, _teamId))
        {
            score += FightScore;
        }

        // идём к золоту
        if (currentMove is { WithCoin: false, WithBigCoin: false } && 
            _bigCoinPositions.Contains(currentMove.To.Position))
        {
            score += TakeBigCoinScore / 2;
        }
        
        if (currentMove is { WithCoin: false, WithBigCoin: false } && 
            _coinPositions.Contains(currentMove.To.Position))
        {
            score += TakeCoinScore / 2;
        }
        
        // открываем новые клетки
        if (_unknownPositions.Contains(currentMove.To.Position))
        {
            score += OpenScore;
        }
        
        // залазим на свой корабль
        if (_waterPositions.Contains(currentMove.From.Position))
        {
            if (currentMove.To.Position == _shipPosition)
            {
                score += BackFromWaterScore;
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