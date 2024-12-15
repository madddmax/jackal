using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Actions;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Jackal.Core.Players;

namespace Jackal.Core;

public class Game
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

    private readonly List<Move> _availableMoves;
    private readonly List<IGameAction> _actions;

    /// <summary>
    /// ИД игры
    /// </summary>
    public readonly Guid GameId = Guid.NewGuid();
    
    /// <summary>
    /// Индекс набора игровых сообщений
    /// </summary>
    private int MessagesKitIndex => Math.Abs(GameId.GetHashCode() % GameMessages.Kit.Length);

    /// <summary>
    /// Игровое сообщение
    /// </summary>
    public string GameMessage { get; private set; }
    
    public Game(IPlayer[] players, IMapGenerator mapGenerator, int mapSize, int piratesPerPlayer)
    {
        _players = players;

        _availableMoves = new List<Move>();
        _actions = new List<IGameAction>();

        foreach (var player in _players)
        {
            player.OnNewGame();
        }

        Board = new Board(players, mapGenerator, mapSize, piratesPerPlayer);
        GameMessage = GameMessages.Kit[MessagesKitIndex][0];
    }

    public void Turn()
    {
        GetAvailableMoves(CurrentTeamId);

        NeedSubTurnPirate = null;
        PrevSubTurnPosition = null;

        if (_availableMoves.Count > 0)
        {
            // есть возможные ходы
            var gameState = new GameState
            {
                AvailableMoves = _availableMoves.ToArray(),
                Board = Board,
                GameId = GameId,
                TurnNumber = TurnNo,
                TeamId = CurrentTeamId
            };
            var (moveNum, pirateId) = CurrentPlayer.OnMove(gameState);
                
            var from = _availableMoves[moveNum].From;
            var currentTeamPirates = Board.Teams[CurrentTeamId].Pirates
                .Where(x => x.IsActive)
                .ToList();
            
            var pirate =
                currentTeamPirates.FirstOrDefault(x => x.Id == pirateId && x.Position == from)
                ?? currentTeamPirates.First(x => x.Position == from);
                
            IGameAction action = _actions[moveNum];
            action.Act(this, pirate);
        }

        if (NeedSubTurnPirate == null)
        {
            //также протрезвляем всех пиратов, которые начали бухать раньше текущего хода
            foreach (Pirate pirate in Board.Teams[CurrentTeamId].Pirates.Where(x => x.IsDrunk && x.DrunkSinceTurnNo < TurnNo))
            {
                pirate.DrunkSinceTurnNo = null;
                pirate.IsDrunk = false;
            }

            TurnNo++;
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
            GameMessage = gameMessages[TurnNo / _players.Length % gameMessages.Length];
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
        GetAvailableMoves(CurrentTeamId);
        return _availableMoves;
    }

    private void GetAvailableMoves(int teamId)
    {
        _availableMoves.Clear();
        _actions.Clear();
            
        Team team = Board.Teams[teamId];

        IEnumerable<Pirate> activePirates = NeedSubTurnPirate != null
            ? new[] { NeedSubTurnPirate }
            : team.Pirates.Where(x => x.IsActive);

        var targets = new List<AvailableMove>();
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
            if (_availableMoves.Exists(x => x == move))
                continue;

            _availableMoves.Add(move);
            _actions.Add(availableMove.ActionList);
        }
    }

    /// <summary>
    /// Конец игры
    /// </summary>
    public bool IsGameOver { get; private set; }

    /// <summary>
    /// Текущий ход - определяет какая команда ходит
    /// </summary>
    public int TurnNo { get; private set; }
        
    /// <summary>
    /// Последний ход когда производилось действие:
    /// открытие новой клетки или перенос монеты 
    /// </summary>
    public int LastActionTurnNo { get; internal set; }

    /// <summary>
    /// ИД команды которая ходит
    /// </summary>
    public int CurrentTeamId => TurnNo % _players.Length;

    /// <summary>
    /// Игрок который ходит
    /// </summary>
    public IPlayer CurrentPlayer => _players[CurrentTeamId];

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
        if (orderedTeamCoins.Count > 1)
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
        if (TurnNo - 50 * _players.Length > LastActionTurnNo)
        {
            return (true, "яичинга");
        }

        return (false, "");
    }
}