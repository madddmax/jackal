using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Actions;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Jackal.Core.Players;
using Newtonsoft.Json;

namespace Jackal.Core;

public record GameRequest(
    // данные по карте todo подумать об объединении
    int MapSize,
    IMapGenerator MapGenerator,
    
    // данные по игрокам
    IPlayer[] Players,
    GameModeType GameMode = GameModeType.FreeForAll,
    int PiratesPerPlayer = 3
);

public class Game : ICompletable
{
    private readonly IPlayer[] _players;

    public readonly Board Board;
        
    /// <summary>
    /// Открытое золото на карте
    /// </summary>
    public int CoinsOnMap;

    /// <summary>
    /// Потерянные монеты
    /// </summary>
    public int LostCoins;

    /// <summary>
    /// Режим игры
    /// </summary>
    public readonly GameModeType GameMode;
    
    /// <summary>
    /// Рэндом для выбора игровых сообщений
    /// </summary>
    public readonly int MessagesKitRandom = new Random().Next();
    
    /// <summary>
    /// Индекс набора игровых сообщений
    /// </summary>
    [JsonIgnore]
    private int MessagesKitIndex => Math.Abs(MessagesKitRandom % GameMessages.Kit.Length);

    /// <summary>
    /// Игровое сообщение
    /// </summary>
    [JsonIgnore]
    public string GameMessage { get; private set; }
    
    public Game(GameRequest request)
    {
        _players = request.Players;
        GameMode = request.GameMode;

        foreach (var player in _players)
        {
            player.OnNewGame();
        }

        Board = new Board(request);
        GameMessage = GameMessages.Kit[MessagesKitIndex][0];
    }

    public void Turn()
    {
        var result = GetAvailableMoves(CurrentTeamId);

        NeedSubTurnPirate = null;
        PrevSubTurnPosition = null;

        if (result.AvailableMoves.Count > 0)
        {
            // есть возможные ходы
            var gameState = new GameState
            {
                AvailableMoves = result.AvailableMoves.ToArray(),
                Board = Board,
                TeamId = CurrentTeamId,
                UserId = Board.Teams[CurrentPlayerIndex].UserId
            };
            var (moveNum, pirateId) = CurrentPlayer.OnMove(gameState);

            var move = result.AvailableMoves[moveNum];
            var currentTeamPirates = Board.Teams[CurrentTeamId].Pirates
                .Where(x => !x.IsDrunk && !x.IsInHole && x.Position == move.From)
                .Where(x => (!x.IsInTrap && !move.WithRumBottle) || move.WithRumBottle)
                .ToList();

            var pirate = currentTeamPirates.FirstOrDefault(x => x.Id == pirateId) ?? currentTeamPirates.First();
            IGameAction action = result.Actions[moveNum];
            action.Act(this, pirate);
        }

        if (NeedSubTurnPirate == null)
        {
            //также протрезвляем всех пиратов, которые начали бухать раньше текущего хода
            foreach (Pirate pirate in Board.Teams[CurrentTeamId].Pirates.Where(x => x.IsDrunk && x.DrunkSinceTurnNumber < TurnNumber))
            {
                pirate.DrunkSinceTurnNumber = null;
                pirate.IsDrunk = false;
            }

            TurnNumber++;
            SubTurn.Clear();
        }

        (IsGameOver, string gameOverMessage) = CheckGameOver();

        if (IsGameOver)
        {
            var maxCoins = Board.Teams.Max(x => x.Coins);
            var winners = Board.Teams.Length == 1 || Board.Teams.Any(x => x.Coins != maxCoins)
                ? string.Join(" и ", Board.Teams.Where(x => x.Coins == maxCoins).Select(x => x.Name))
                : "дружбы";
            
            GameMessage = $"Победа {winners} путём {gameOverMessage}!";
        }
        else
        {
            var gameMessages = GameMessages.Kit[MessagesKitIndex];
            GameMessage = gameMessages[TurnNumber / _players.Length % gameMessages.Length];
        }
    }

    /// <summary>
    /// TODO-MAD является результатом действия -
    /// Пират которому требуется дополнительный ход 
    /// </summary>
    public Pirate? NeedSubTurnPirate { private get; set; }
        
    /// <summary>
    /// TODO-MAD является результатом действия -
    /// Предыдущая позиция пирата которому требуется дополнительный ход
    /// </summary>     
    public TilePosition? PrevSubTurnPosition { get; set; }

    /// <summary>
    /// TODO-MAD является результатом действия -
    /// Состояние дополнительного хода
    /// </summary>
    public SubTurnState SubTurn { get; } = new();
    
    public List<Move> GetAvailableMoves()
    {
        var result = GetAvailableMoves(CurrentTeamId);
        return result.AvailableMoves;
    }

    private AvailableMoveResult GetAvailableMoves(int teamId)
    {
        var result = new AvailableMoveResult();
        var targets = new List<AvailableMove>();
            
        Team team = Board.Teams[teamId];
        if (team.RumBottles > 0 && NeedSubTurnPirate == null)
        {
            IEnumerable<Pirate> piratesWithRumBottles = team.Pirates.Where(x => x.IsInTrap || x.Position.Level > 0);
            foreach (var pirate in piratesWithRumBottles)
            {
                AvailableMovesTask task = new AvailableMovesTask(teamId, pirate.Position, pirate.Position);
                List<AvailableMove> moves = Board.GetAllAvailableMoves(
                    task,
                    task.Source,
                    task.Prev,
                    new SubTurnState { DrinkRumBottle = true }
                );
                foreach (var move in moves)
                {
                    var drinkRumBottleAction = new DrinkRumBottleAction();
                    move.ActionList.AddFirstAction(drinkRumBottleAction);
                    move.MoveType = move.MoveType switch
                    {
                        MoveType.WithCoin => MoveType.WithRumBottleAndCoin,
                        MoveType.WithBigCoin => MoveType.WithRumBottleAndBigCoin,
                        _ => MoveType.WithRumBottle
                    };
                }
                
                targets.AddRange(moves);
            }
        }
        
        IEnumerable<Pirate> activePirates = NeedSubTurnPirate != null
            ? new[] { NeedSubTurnPirate }
            : team.Pirates.Where(x => x.IsActive);
        
        foreach (var pirate in activePirates)
        {
            TilePosition prev = PrevSubTurnPosition ?? pirate.Position;
            AvailableMovesTask task = new AvailableMovesTask(teamId, pirate.Position, prev);
            List<AvailableMove> moves = Board.GetAllAvailableMoves(
                task,
                task.Source,
                task.Prev,
                SubTurn
            );
            targets.AddRange(moves);
        }

        foreach (var availableMove in targets)
        {
            var move = availableMove.ToMove;
            if (result.AvailableMoves.Exists(x => x == move))
                continue;

            result.AvailableMoves.Add(move);
            result.Actions.Add(availableMove.ActionList);
        }

        return result;
    }

    /// <summary>
    /// Конец игры
    /// </summary>
    public bool IsGameOver { get; private set; }

    public bool IsCompleted => IsGameOver;

    /// <summary>
    /// Текущий ход - определяет какая команда ходит
    /// </summary>
    public int TurnNumber { get; private set; }
        
    /// <summary>
    /// Последний ход когда производилось действие:
    /// открытие новой клетки или перенос монеты 
    /// </summary>
    public int LastActionTurnNumber { get; internal set; }

    /// <summary>
    /// ИД команды пиратов чей ход
    /// </summary>
    public int CurrentTeamId => TurnNumber % _players.Length;

    /// <summary>
    /// Индекс игрока чей ход,
    /// отличается от ИД команды при розыгрыше хи-хи травы
    /// </summary>
    public int CurrentPlayerIndex => (TurnNumber + (SubTurn.CannabisTurnCount > 0 ? 1 : 0)) % _players.Length;
    
    /// <summary>
    /// Игрок который ходит
    /// </summary>
    public IPlayer CurrentPlayer => _players[CurrentPlayerIndex];

    /// <summary>
    /// Убрать пирата с карты
    /// </summary>
    public void KillPirate(Pirate pirate)
    {
        int teamId = pirate.TeamId;
        Board.Teams[teamId].Pirates = Board.Teams[teamId].Pirates.Where(x => x != pirate).ToArray();
        var tile = Board.Map[pirate.Position];
        tile.Pirates.Remove(pirate);

        Board.DeadPirates ??= [];
        Board.DeadPirates.Add(pirate);
    }

    /// <summary>
    /// Добавить нового пирата на карту на открытую клетку
    /// </summary>
    public void AddPirate(int teamId, TilePosition position, PirateType type)
    {
        if (Board.Map[position.Position].Type == TileType.Unknown)
        {
            throw new Exception("Tile must not be Unknown");
        }
            
        var newPirate = new Pirate(teamId, position, type);
        Board.Teams[teamId].Pirates = Board.Teams[teamId].Pirates.Concat(new[] { newPirate }).ToArray();
            
        var tile = Board.Map[position];
        tile.Pirates.Add(newPirate);
    }

    /// <summary>
    /// Переместить пирата на его корабль
    /// </summary>
    public void MovePirateToTheShip(Pirate pirate)
    {
        var team = Board.Teams[pirate.TeamId];
        var pirateTileLevel = Board.Map[pirate.Position];
            
        pirate.Position = new TilePosition(team.ShipPosition);
        Board.Map[team.ShipPosition].Pirates.Add(pirate);
        pirateTileLevel.Pirates.Remove(pirate);
            
        pirate.ResetEffects();
    }

    private void MovePirateToPosition(Pirate pirate, Position position)
    {
        var pirateTileLevel = Board.Map[pirate.Position];
            
        pirate.Position = new TilePosition(position);
        Board.Map[position].Pirates.Add(pirate);
        pirateTileLevel.Pirates.Remove(pirate);
        
        pirate.ResetEffects();
    }

    public void SwapPiratePosition(Tile firstTile, Tile secondTile)
    {
        var pirates = new List<Pirate>(secondTile.Pirates);
        foreach (var movedPirate in firstTile.Pirates)
        {
            MovePirateToPosition(movedPirate, secondTile.Position);
        }

        foreach (var movedPirate in pirates)
        {
            MovePirateToPosition(movedPirate, firstTile.Position);
        }
    }

    private (bool GameOver, string Message) CheckGameOver()
    {
        var orderedTeamCoins = Board.Teams
            .Select(x => x.Coins)
            .OrderByDescending(x => x)
            .ToList();

        // игра на несколько игроков
        if (orderedTeamCoins.Count == 4 && 
            GameMode == GameModeType.TwoPlayersInTeam)
        {
            // свободное золото
            int freeCoins = Board.Generator.TotalCoins - LostCoins - orderedTeamCoins.Sum() / 2;
            
            // игрок затащил большую часть монет
            int firstTeamCoins = orderedTeamCoins[0];
            int secondTeamCoins = orderedTeamCoins[2] + freeCoins;
            if (firstTeamCoins > secondTeamCoins)
            {
                return (true, "доминирования по золоту");
            }
        }
        else if (orderedTeamCoins.Count > 1)
        {
            // свободное золото
            int freeCoins = Board.Generator.TotalCoins - LostCoins - orderedTeamCoins.Sum();
            
            // игрок затащил большую часть монет
            int firstTeamCoins = orderedTeamCoins[0];
            int secondTeamCoins = orderedTeamCoins[1] + freeCoins;
            if (firstTeamCoins > secondTeamCoins)
            {
                return (true, "доминирования по золоту");
            }
        }

        // все клетки открыты и нет золота на карте
        var allTilesOpen = !Board.AllTiles(x => x.Type == TileType.Unknown).Any();
        if (allTilesOpen && CoinsOnMap == 0)
        {
            return (true, "исследования карты");
        }

        // закончились пираты
        if (Board.AllPirates.All(p => p.IsDisable))
        {
            return (true, "конца всех пиратов");
        }
            
        // защита от яичинга (ходов без открытия клеток или переноса монет)
        if (TurnNumber - 50 * _players.Length > LastActionTurnNumber)
        {
            return (true, "яичинга");
        }

        return (false, "");
    }
}