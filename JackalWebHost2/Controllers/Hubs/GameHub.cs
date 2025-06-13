using Jackal.Core;
using Jackal.Core.MapGenerator.TilesPack;
using JackalWebHost2.Controllers.Models;
using JackalWebHost2.Controllers.Models.Game;
using JackalWebHost2.Controllers.Models.Services;
using JackalWebHost2.Data.Entities;
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

    private const string CALLBACK_GET_NET_GAME_DATA = "GetNetGameData";
    private const string CALLBACK_GET_ACTIVE_NET_GAMES = "GetActiveNetGames";

    private readonly IGameService _gameService;
    private readonly IStateRepository<Game> _gameStateRepository;
    private readonly IStateRepository<NetGameSettings> _netgameStateRepository;
    private readonly Random _random;

    
    public GameHub(
        IGameService gameService, 
        IStateRepository<Game> gameStateRepository,
        IStateRepository<NetGameSettings> netgameStateRepository)
    {
        _gameService = gameService;
        _gameStateRepository = gameStateRepository;
        _netgameStateRepository = netgameStateRepository;
        _random = new Random(DateTime.Now.Millisecond);
    }

    public override async Task OnConnectedAsync()
    {
        var user = FastAuthJwtBearerHelper.ExtractUser(Context.User);
        await Clients.Caller.SendAsync(CALLBACK_NOTIFY, $"{user.Login} вошел в игру");
        await Clients.Caller.SendAsync(CALLBACK_GET_ACTIVE_GAMES, new AllActiveGamesResponse
        {
            GamesEntries = _gameStateRepository.GetEntries().Select(ToActiveGame).ToList()
        });
        await Clients.Caller.SendAsync(CALLBACK_GET_ACTIVE_NET_GAMES, new AllActiveGamesResponse
        {
            GamesEntries = _netgameStateRepository.GetEntries().Select(ToActiveGame).ToList()
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

        await Clients.All.SendAsync(CALLBACK_GET_ACTIVE_GAMES, new AllActiveGamesResponse
        {
            GamesEntries = _gameStateRepository.GetEntries().Select(ToActiveGame).ToList()
        });
        _gameStateRepository.ResetChanges();

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
    public async Task StartPublic(NetGameRequest request)
    {
        var user = FastAuthJwtBearerHelper.ExtractUser(Context.User);
        var netGame = _netgameStateRepository.GetObject(request.Id);
        if (netGame?.CreatorId != user.Id) return;

        var startGameModel = new StartGameModel { Settings = request.Settings };
        var result = await _gameService.StartGame(user, startGameModel);

        netGame.GameId = result.GameId;
        _netgameStateRepository.UpdateObject(netGame.Id, netGame);

        await Clients.Group(GetNetGroupName(netGame.Id)).SendAsync(CALLBACK_GET_NET_GAME_DATA, new NetGameResponse
        {
            Id = netGame.Id,
            GameId = result.GameId,
            Settings = netGame.Settings,
            Viewers = netGame.Users
        });

        // скрываем завершённую сетевую игру
        await Clients.All.SendAsync(CALLBACK_GET_ACTIVE_NET_GAMES, new AllActiveGamesResponse
        {
            GamesEntries = _netgameStateRepository.GetEntries().Select(ToActiveGame).ToList()
        });
        _netgameStateRepository.ResetChanges();

        // показываем новую публичную игру
        await Clients.All.SendAsync(CALLBACK_GET_ACTIVE_GAMES, new AllActiveGamesResponse
        {
            GamesEntries = _gameStateRepository.GetEntries().Select(ToActiveGame).ToList()
        });
        _gameStateRepository.ResetChanges();
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


    /// <summary>
    /// Старт новой сетевой игры
    /// </summary>
    public async Task NetStart(NetGameRequest request)
    {
        var user = FastAuthJwtBearerHelper.ExtractUser(Context.User);
        var netGame = new NetGameSettings
        {
            Id = _random.NextInt64(1, 100_000_000),
            CreatorId = user.Id,
            Users = new HashSet<long>{user.Id},
            Settings = request.Settings
        };
        _netgameStateRepository.CreateObject(user, netGame.Id, netGame);

        await Groups.AddToGroupAsync(Context.ConnectionId, GetNetGroupName(netGame.Id));
        await Clients.Group(GetNetGroupName(netGame.Id)).SendAsync(CALLBACK_GET_NET_GAME_DATA, new NetGameResponse
        {
            Id = netGame.Id,
            Settings = netGame.Settings,
            Viewers = netGame.Users
        });

        await Clients.All.SendAsync(CALLBACK_GET_ACTIVE_NET_GAMES, new AllActiveGamesResponse
        {
            GamesEntries = _netgameStateRepository.GetEntries().Select(ToActiveGame).ToList()
        });
        _netgameStateRepository.ResetChanges();
    }


    /// <summary>
    /// Изменение сетевой игры
    /// </summary>
    public async Task NetUpdate(NetGameRequest request)
    {
        var user = FastAuthJwtBearerHelper.ExtractUser(Context.User);
        var netGame = _netgameStateRepository.GetObject(request.Id);
        if (netGame?.CreatorId != user.Id) return;

        netGame.Settings = request.Settings;
        _netgameStateRepository.UpdateObject(netGame.Id, netGame);

        await Clients.Group(GetNetGroupName(netGame.Id)).SendAsync(CALLBACK_GET_NET_GAME_DATA, new NetGameResponse
        {
            Id = netGame.Id,
            Settings = netGame.Settings,
            Viewers = netGame.Users
        });
     }

    /// <summary>
    /// Присоединиться к сетевой игре
    /// </summary>
    public async Task NetJoin(NetUserRequest request)
    {
        var user = FastAuthJwtBearerHelper.ExtractUser(Context.User);
        var netGame = _netgameStateRepository.GetObject(request.Id);
        if (netGame == null) return;

        netGame.Users.Add(user.Id);
        _netgameStateRepository.UpdateObject(netGame.Id, netGame);

        await Groups.AddToGroupAsync(Context.ConnectionId, GetNetGroupName(netGame.Id));
        await Clients.Group(GetNetGroupName(netGame.Id)).SendAsync(CALLBACK_GET_NET_GAME_DATA, new NetGameResponse
        {
            Id = netGame.Id,
            Settings = netGame.Settings,
            Viewers = netGame.Users
        });
    }

    /// <summary>
    /// Покинуть сетевую игру
    /// </summary>
    public async Task NetLeave(NetUserRequest request)
    {
        var user = FastAuthJwtBearerHelper.ExtractUser(Context.User);
        var netGame = _netgameStateRepository.GetObject(request.Id);
        if (netGame == null) return;

        netGame.Users.Remove(user.Id);
        _netgameStateRepository.UpdateObject(netGame.Id, netGame);

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetNetGroupName(netGame.Id));
        await Clients.Group(GetNetGroupName(netGame.Id)).SendAsync(CALLBACK_GET_NET_GAME_DATA, new NetGameResponse
        {
            Id = netGame.Id,
            Settings = netGame.Settings,
            Viewers = netGame.Users
        });
    }

    private string GetNetGroupName(long netGameId)
    {
        return $"netgrp{netGameId}";
    }

    private string GetGroupName(long gameId)
    {
        return $"grp{gameId}";
    }

    private ActiveGameInfo ToActiveGame(CacheEntry entry)
    {
        return new ActiveGameInfo
        {
            GameId = entry.ObjectId,
            Creator = entry.Creator,
            TimeStamp = entry.TimeStamp
        };
    }
}