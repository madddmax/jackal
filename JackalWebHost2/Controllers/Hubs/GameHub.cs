using Jackal.Core.MapGenerator.TilesPack;
using JackalWebHost2.Controllers.Models;
using JackalWebHost2.Models;
using JackalWebHost2.Services;
using Microsoft.AspNetCore.SignalR;

namespace JackalWebHost2.Controllers.Hubs;

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
        await Clients.Caller.SendAsync(CALLBACK_NOTIFY, $"{Context.ConnectionId} вошел в игру");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.Others.SendAsync(CALLBACK_NOTIFY, $"{Context.ConnectionId} покинул игру");
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Старт новой игры
    /// </summary>
    public async Task Start(StartGameRequest request)
    {
        var result = await _gameService.StartGame(new StartGameModel
        {
            GameName = request.GameName,
            Settings = request.Settings
        });

        var packName = TilesPackFactory.CheckName(request.Settings.TilesPackName);
        await Clients.Caller.SendAsync(CALLBACK_GET_START_DATA, new StartGameResponse
        {
            GameName = result.GameName,
            GameMode = result.GameMode,
            TilesPackName = packName,
            Pirates = result.Pirates,
            Map = result.Map,
            MapId = result.MapId,
            Stats = result.Statistics,
            Teams = result.Teams,
            Moves = result.Moves
        });

        // todo - подумать про пересчет ходов компа в ядре
        if (!result.Statistics.IsGameOver && (!result.Teams[result.Statistics.CurrentTeamId].IsHuman || result.Moves.Count == 0))
        {
            await Move(new TurnGameRequest
            {
                GameName = request.GameName
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
            GameName = request.GameName,
            TurnNum = request.TurnNum,
            PirateId = request.PirateId
        });

        await Clients.Caller.SendAsync(CALLBACK_GET_MOVE_CHANGES, new TurnGameResponse
        {
            PirateChanges = result.PirateChanges,
            Changes = result.Changes,
            Stats = result.Statistics,
            TeamChanges = result.Teams.Select(t => new TeamChange(t)).ToList(),
            Moves = result.Moves
        });

        // todo - подумать про пересчет ходов компа в ядре, тогда можно сразу отдавать TeamChange вместо DrawTeam
        if (!result.Statistics.IsGameOver && (!result.Teams[result.Statistics.CurrentTeamId].IsHuman || result.Moves.Count == 0))
        {
            await Move(new TurnGameRequest
            {
                GameName = request.GameName
            });
        }
    }
}