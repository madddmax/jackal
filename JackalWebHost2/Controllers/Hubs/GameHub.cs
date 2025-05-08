using Jackal.Core.MapGenerator.TilesPack;
using JackalWebHost2.Controllers.Models;
using JackalWebHost2.Controllers.Models.Services;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Infrastructure.Auth;
using JackalWebHost2.Models;
using JackalWebHost2.Services;
using Microsoft.AspNetCore.SignalR;

namespace JackalWebHost2.Controllers.Hubs;

[FastAuth]
public class GameHub : Hub
{
    private const string CALLBACK_NOTIFY = "Notify";
    private const string CALLBACK_LOAD_GAME_DATA = "LoadGameData";
    private const string CALLBACK_GET_START_DATA = "GetStartData";
    private const string CALLBACK_GET_MOVE_CHANGES = "GetMoveChanges";
    private const string CALLBACK_GET_ACTIVE_GAMES = "GetActiveGames";

    private readonly IGameService _gameService;
    private readonly IGameStateRepository _gameStateRepository;

    public GameHub(IGameService gameService, IGameStateRepository gameStateRepository)
    {
        _gameService = gameService;
        _gameStateRepository = gameStateRepository;
    }

    public override async Task OnConnectedAsync()
    {
        var user = FastAuthJwtBearerHelper.ExtractUser(Context.User);
        await Clients.Caller.SendAsync(CALLBACK_NOTIFY, $"{user.Login} вошел в игру");
        await Clients.Caller.SendAsync(CALLBACK_GET_ACTIVE_GAMES, new AllActiveGamesResponse
        {
            GamesKeys = _gameStateRepository.GetAllKeys(),
            GamesEntries = _gameStateRepository.GetGamesEntries()
        });
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = FastAuthJwtBearerHelper.ExtractUser(Context.User);
        await Clients.Others.SendAsync(CALLBACK_NOTIFY, $"{user.Login} покинул игру");
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Загрузка существующей игры
    /// </summary>
    public async Task Load(LoadGameRequest request)
    {
        var user = FastAuthJwtBearerHelper.ExtractUser(Context.User);
        var result = await _gameService.LoadGame(user.Id, request.GameId);

        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(result.GameId));
        await Clients.Caller.SendAsync(CALLBACK_LOAD_GAME_DATA, new LoadGameResponse
        {
            GameId = result.GameId,
            GameMode = result.GameMode,
            TilesPackName = result.TilesPackName,
            Pirates = result.Pirates,
            Map = result.Map,
            MapId = result.MapId,
            Stats = result.Statistics,
            Teams = result.Teams,
            Moves = result.Moves
        });

        if (!result.Statistics.IsGameOver && result.Moves.Count == 0)
        {
            await Move(new TurnGameRequest
            {
                GameId = result.GameId
            });
        }
    }
    
    /// <summary>
    /// Старт новой игры
    /// </summary>
    public async Task Start(StartGameRequest request)
    {
        var user = FastAuthJwtBearerHelper.ExtractUser(Context.User);
        var startGameModel = new StartGameModel { Settings = request.Settings };
        var result = await _gameService.StartGame(user, startGameModel);

        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(result.GameId));
        await Clients.Group(GetGroupName(result.GameId)).SendAsync(CALLBACK_GET_START_DATA, new StartGameResponse
        {
            GameId = result.GameId,
            GameMode = result.GameMode,
            TilesPackName = TilesPackFactory.CheckName(request.Settings.TilesPackName),
            Pirates = result.Pirates,
            Map = result.Map,
            MapId = result.MapId,
            Stats = result.Statistics,
            Teams = result.Teams,
            Moves = result.Moves
        });

        if (_gameStateRepository.HasGamesChanges())
        {
            await Clients.All.SendAsync(CALLBACK_GET_ACTIVE_GAMES, new AllActiveGamesResponse
            {
                GamesKeys = _gameStateRepository.GetAllKeys(),
                GamesEntries = _gameStateRepository.GetGamesEntries()
            });
            _gameStateRepository.ResetGamesChanges();
        }

        if (!result.Statistics.IsGameOver && result.Moves.Count == 0)
        {
            await Move(new TurnGameRequest
            {
                GameId = result.GameId
            });
        }
    }

    /// <summary>
    /// Ход игры
    /// </summary>
    public async Task Move(TurnGameRequest request)
    {
        var user = FastAuthJwtBearerHelper.ExtractUser(Context.User);
        var turnGameModel = new TurnGameModel
        {
            GameId = request.GameId,
            TurnNum = request.TurnNum,
            PirateId = request.PirateId
        };
        var result = await _gameService.MakeGameTurn(user.Id, turnGameModel);

        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(request.GameId));
        await Clients.Group(GetGroupName(request.GameId)).SendAsync(CALLBACK_GET_MOVE_CHANGES, new TurnGameResponse
        {
            PirateChanges = result.PirateChanges,
            Changes = result.Changes,
            Stats = result.Statistics,
            TeamScores = result.TeamScores,
            Moves = result.Moves
        });
        
        if (!result.Statistics.IsGameOver && result.Moves.Count == 0)
        {
            await Move(new TurnGameRequest
            {
                GameId = request.GameId
            });
        }
    }


    private string GetGroupName(long gameId)
    {
        return $"grp{gameId}";
    }
}