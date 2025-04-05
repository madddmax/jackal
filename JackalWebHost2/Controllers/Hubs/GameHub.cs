using Jackal.Core.MapGenerator.TilesPack;
using JackalWebHost2.Controllers.Models;
using JackalWebHost2.Infrastructure.Auth;
using JackalWebHost2.Models;
using JackalWebHost2.Services;
using Microsoft.AspNetCore.SignalR;

namespace JackalWebHost2.Controllers.Hubs;

[FastAuth]
public class GameHub : Hub
{
    private const string CALLBACK_NOTIFY = "Notify";
    private const string CALLBACK_GET_START_DATA = "GetStartData";
    private const string CALLBACK_GET_MOVE_CHANGES = "GetMoveChanges";

    private readonly IGameService _gameService;

    public GameHub(IGameService gameService)
    {
        _gameService = gameService;
    }

    public override async Task OnConnectedAsync()
    {
        var user = FastAuthJwtBearerHelper.ExtractUser(Context.User);
        await Clients.Caller.SendAsync(CALLBACK_NOTIFY, $"{user.Login} вошел в игру");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = FastAuthJwtBearerHelper.ExtractUser(Context.User);
        await Clients.Others.SendAsync(CALLBACK_NOTIFY, $"{user.Login} покинул игру");
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Старт новой игры
    /// </summary>
    public async Task Start(StartGameRequest request)
    {
        var result = await _gameService.StartGame(new StartGameModel
        {
            Settings = request.Settings
        });

        var packName = TilesPackFactory.CheckName(request.Settings.TilesPackName);
        await Clients.Caller.SendAsync(CALLBACK_GET_START_DATA, new StartGameResponse
        {
            GameId = result.GameId,
            GameMode = result.GameMode,
            TilesPackName = packName,
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
    /// Ход игры
    /// </summary>
    public async Task Move(TurnGameRequest request)
    {
        var result = await _gameService.MakeGameTurn(new TurnGameModel
        {
            GameId = request.GameId,
            TurnNum = request.TurnNum,
            PirateId = request.PirateId
        });

        await Clients.Caller.SendAsync(CALLBACK_GET_MOVE_CHANGES, new TurnGameResponse
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
}