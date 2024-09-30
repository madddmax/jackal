using Jackal.Core;
using Jackal.Core.MapGenerator;
using Jackal.Core.Players;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Exceptions;
using JackalWebHost2.Models;
using GameState = JackalWebHost2.Models.GameState;

namespace JackalWebHost2.Services;

public class GameService : IGameService
{
    private readonly IGameStateRepository _gameStateRepository;
    private readonly IDrawService _drawService;
    
    public GameService(IGameStateRepository gameStateRepository, IDrawService drawService)
    {
        _gameStateRepository = gameStateRepository;
        _drawService = drawService;
    }
    
    public async Task<StartGameResult> StartGame(StartGameModel request)
    {
        // todo validate game name
        
        GameState gameState = new GameState();
        GameSettings gameSettings = request.Settings;

        IPlayer[] gamePlayers = new IPlayer[gameSettings.Players.Length];
        int index = 0;

        foreach (var player in gameSettings.Players)
        {
            gamePlayers[index++] = player switch
            {
                "robot" => new RandomPlayer(),
                "human" => new WebHumanPlayer(),
                _ => new EasyPlayer()
            };
        }

        gameSettings.MapId ??= new Random().Next();

        // TODO-MIKE для ручной отладки можно использовать закомментированные генераторы карт
        int mapSize = gameSettings.MapSize ?? 5;
        IMapGenerator mapGenerator = new ClassicMapGenerator(gameSettings.MapId.Value, mapSize);
        // mapGenerator = new OneTileMapGenerator(new TileParams(TileType.Trap));
        // mapGenerator = new TwoTileMapGenerator(
        //     new TileParams(TileType.Arrow) { ArrowsCode = ArrowsCodesHelper.OneArrowUp },
        //     new TileParams(TileType.Crocodile));
            
        int piratesPerPlayer = 3;
        gameState.board = new Board(gamePlayers, mapGenerator, mapSize, piratesPerPlayer);
        gameState.game = new Game(gamePlayers, gameState.board);

        await _gameStateRepository.CreateGameState(request.GameName, gameState);
        
        var map = _drawService.Map(gameState.board);

        List<PirateChange> pirateChanges = [];
        foreach (var pirate in gameState.game.Board.AllPirates)
        {
            pirateChanges.Add(new PirateChange(pirate));
        }

        return new StartGameResult
        {
            GameName = request.GameName,
            Pirates = pirateChanges,
            Map = map,
            MapId = gameSettings.MapId.Value,
            Statistics = _drawService.GetStatistics(gameState.game),
            Moves = _drawService.GetAvailableMoves(gameState.game)
        };
    }
    
    public async Task<TurnGameResult> MakeGameTurn(TurnGameModel request)
    {
        var gameState = await _gameStateRepository.GetGameState(request.GameName);
        if (gameState == null)
        {
            throw new GameNotFoundException();
        }

        var prevBoardStr = JsonHelper.SerializeWithType(gameState.board);
            
        if (gameState.game.CurrentPlayer is WebHumanPlayer && request.TurnNum.HasValue)
        {
            gameState.game.CurrentPlayer.SetHumanMove(request.TurnNum.Value, request.PirateId);
        }

        gameState.game.Turn();
        await _gameStateRepository.UpdateGameState(request.GameName, gameState);
        
        var prevBoard = JsonHelper.DeserializeWithType<Board>(prevBoardStr);
        
        (List<PirateChange> pirateChanges, List<TileChange> tileChanges) = _drawService.Draw(gameState.board, prevBoard);
        
        return new TurnGameResult
        {
            PirateChanges = pirateChanges,
            Changes = tileChanges,
            Statistics = _drawService.GetStatistics(gameState.game),
            Moves = _drawService.GetAvailableMoves(gameState.game)
        };
    }
}