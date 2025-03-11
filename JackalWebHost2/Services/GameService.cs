using Jackal.Core;
using Jackal.Core.MapGenerator;
using Jackal.Core.Players;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Exceptions;
using JackalWebHost2.Models;
using JackalWebHost2.Models.Player;

namespace JackalWebHost2.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IDrawService _drawService;
    
    public GameService(IGameRepository gameRepository, IDrawService drawService)
    {
        _gameRepository = gameRepository;
        _drawService = drawService;
    }
    
    public async Task<StartGameResult> StartGame(StartGameModel request)
    {
        // todo validate game name
        
        GameSettings gameSettings = request.Settings;
        IPlayer[] gamePlayers = new IPlayer[gameSettings.Players.Length];
        int index = 0;

        foreach (var player in gameSettings.Players)
        {
            gamePlayers[index++] = player.Type switch
            {
                PlayerType.Robot => new RandomPlayer(),
                PlayerType.Human => new WebHumanPlayer(),
                _ => new EasyPlayer()
            };
        }

        gameSettings.MapId ??= new Random().Next();

        // для ручной отладки можно использовать закомментированные генераторы карт
        int mapSize = gameSettings.MapSize ?? 5;
        IMapGenerator mapGenerator = new RandomMapGenerator(gameSettings.MapId.Value, mapSize, gameSettings.TilesPackName);
        //mapGenerator = new OneTileMapGenerator(new TileParams(TileType.Airplane));
        // mapGenerator = new ThreeTileMapGenerator(
        //     new TileParams(TileType.Arrow) { ArrowsCode = ArrowsCodesHelper.ThreeArrows },
        //     new TileParams(TileType.Arrow) { ArrowsCode = ArrowsCodesHelper.FourArrowsDiagonal },
        //     new TileParams(TileType.Quake)
        // );

        var gameMode = gameSettings.GameMode ?? GameModeType.FreeForAll;
        var gameRequest = new GameRequest(mapSize, mapGenerator, gamePlayers, gameMode);
        var game = new Game(gameRequest);

        await _gameRepository.CreateGame(request.GameName, game);
        
        var map = _drawService.Map(game.Board);

        List<PirateChange> pirateChanges = [];
        foreach (var pirate in game.Board.AllPirates)
        {
            pirateChanges.Add(new PirateChange(pirate));
        }

        return new StartGameResult
        {
            GameName = request.GameName,
            GameMode = gameMode,
            Pirates = pirateChanges,
            Map = map,
            MapId = gameSettings.MapId.Value,
            Statistics = _drawService.GetStatistics(game),
            Teams = game.Board.Teams.Select(team => new DrawTeam(team)).ToList(),
            Moves = _drawService.GetAvailableMoves(game)
        };
    }
    
    public async Task<TurnGameResult> MakeGameTurn(TurnGameModel request)
    {
        var game = await _gameRepository.GetGame(request.GameName);
        if (game == null)
        {
            throw new GameNotFoundException();
        }

        if (game.IsGameOver)
        {
            return new TurnGameResult
            {
                PirateChanges = [],
                Changes = [],
                Statistics = _drawService.GetStatistics(game),
                Teams = game.Board.Teams.Select(team => new DrawTeam(team)).ToList(),
                Moves = []
            };
        }
        
        var prevBoardStr = JsonHelper.SerializeWithType(game.Board);
            
        if (game.CurrentPlayer is WebHumanPlayer && request.TurnNum.HasValue)
        {
            game.CurrentPlayer.SetHumanMove(request.TurnNum.Value, request.PirateId);
        }

        game.Turn();
        if (game.IsGameOver)
        {
            game.Board.ShowUnknownTiles();
        }
        
        await _gameRepository.UpdateGame(request.GameName, game);
        var prevBoard = JsonHelper.DeserializeWithType<Board>(prevBoardStr);
        
        return new TurnGameResult
        {
            PirateChanges = _drawService.GetPirateChanges(game.Board, prevBoard),
            Changes = _drawService.GetTileChanges(game.Board, prevBoard),
            Statistics = _drawService.GetStatistics(game),
            Teams = game.Board.Teams.Select(team => new DrawTeam(team)).ToList(),
            Moves = _drawService.GetAvailableMoves(game)
        };
    }
}