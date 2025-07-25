﻿using Jackal.Core;
using Jackal.Core.MapGenerator;
using Jackal.Core.Players;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Exceptions;
using JackalWebHost2.Models;
using JackalWebHost2.Models.Player;

namespace JackalWebHost2.Services;

public class GameService : IGameService
{
    private readonly IStateRepository<Game> _gameStateRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDrawService _drawService;
    
    public GameService(
        IStateRepository<Game> gameStateRepository, 
        IGameRepository gameRepository,
        IUserRepository userRepository,
        IDrawService drawService)
    {
        _gameStateRepository = gameStateRepository;
        _gameRepository = gameRepository;
        _userRepository = userRepository;
        _drawService = drawService;
    }
    
    public async Task<LoadGameResult> LoadGame(long userId, long gameId)
    {
        var game = _gameStateRepository.GetObject(gameId);
        if (game == null)
        {
            throw new GameNotFoundException();
        }
        
        var map = _drawService.Map(game.Board);

        List<PirateChange> pirateChanges = [];
        foreach (var pirate in game.Board.AllPirates)
        {
            pirateChanges.Add(new PirateChange(pirate));
        }
        
        return new LoadGameResult
        {
            GameId = gameId,
            GameMode = game.GameMode,
            TilesPackName = game.Board.Generator.TilesPackName,
            Pirates = pirateChanges,
            Map = map,
            MapId = game.Board.Generator.MapId,
            Statistics = _drawService.GetStatistics(game),
            Teams = game.Board.Teams.Select(team => new DrawTeam(team)).ToList(),
            TeamScores = game.Board.Teams.Select(team => new TeamScore(team)).ToList(),
            Moves = game.CurrentPlayer is HumanPlayer
                ? _drawService.GetAvailableMoves(game)
                : []
        };
    }
    
    public async Task<StartGameResult> StartGame(User user, StartGameModel request)
    {
        GameSettings gameSettings = request.Settings;
        IPlayer[] gamePlayers = new IPlayer[gameSettings.Players.Length];
        int index = 0;

        foreach (var player in gameSettings.Players)
        {
            User? userPlayer = null;
            if (player.Type == PlayerType.Human)
            {
                userPlayer = await _userRepository.GetUser(player.UserId, CancellationToken.None);
            }
            
            gamePlayers[index++] = player.Type switch
            {
                PlayerType.Robot => new RandomPlayer(),
                PlayerType.Robot2 => new EasyPlayer(),
                PlayerType.Robot3 => new OakioPlayer(),
                PlayerType.Human => userPlayer != null 
                    ? new HumanPlayer(userPlayer.Id, userPlayer.Login) 
                    : throw new PlayerNotFoundException(),
                _ => throw new PlayerNotFoundException()
            };
        }

        gameSettings.MapId ??= new Random().Next();

        // для ручной отладки можно использовать закомментированные генераторы карт
        int mapSize = gameSettings.MapSize ?? 5;
        IMapGenerator mapGenerator = new RandomMapGenerator(gameSettings.MapId.Value, mapSize, gameSettings.TilesPackName);
        // mapGenerator = new OneTileMapGenerator(TileParams.Airplane());
        // mapGenerator = new ThreeTileMapGenerator(
        //     TileParams.Airplane(), TileParams.Airplane(), TileParams.Quake()
        // );

        var gameMode = gameSettings.GameMode ?? GameModeType.FreeForAll;
        var gameRequest = new GameRequest(mapSize, mapGenerator, gamePlayers, gameMode);
        var game = new Game(gameRequest);

        var gameId = await _gameRepository.CreateGame(user.Id, game);

        var players = await _userRepository.GetUsers(
            gameSettings.Players.Where(it => it.UserId > 0).Select(it => it.UserId).Distinct().ToArray(), CancellationToken.None);
        _gameStateRepository.CreateObject(user, gameId, game, players.ToHashSet());
        
        var map = _drawService.Map(game.Board);

        List<PirateChange> pirateChanges = [];
        foreach (var pirate in game.Board.AllPirates)
        {
            pirateChanges.Add(new PirateChange(pirate));
        }
        
        return new StartGameResult
        {
            GameId = gameId,
            GameMode = gameMode,
            Pirates = pirateChanges,
            Map = map,
            MapId = gameSettings.MapId.Value,
            Statistics = _drawService.GetStatistics(game),
            Teams = game.Board.Teams.Select(team => new DrawTeam(team)).ToList(),
            Moves = game.CurrentPlayer is HumanPlayer
                ? _drawService.GetAvailableMoves(game)
                : []
        };
    }
    
    public async Task<TurnGameResult> MakeGameTurn(long userId, TurnGameModel request)
    {
        var game = _gameStateRepository.GetObject(request.GameId);
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
                TeamScores = game.Board.Teams.Select(team => new TeamScore(team)).ToList(),
                Moves = []
            };
        }
        
        var prevBoardStr = JsonHelper.SerializeWithType(game.Board);
            
        if (game.CurrentPlayer is IHumanPlayer humanPlayer)
        {
            if (humanPlayer.UserId != userId)
            {
                throw new PlayerNotFoundException();
            }

            if (request.TurnNum.HasValue)
            {
                humanPlayer.SetMove(request.TurnNum.Value, request.PirateId);
            }
        }

        game.Turn();
        if (game.IsGameOver)
        {
            game.Board.ShowUnknownTiles();
        }

        await _gameRepository.UpdateGame(request.GameId, game);
        _gameStateRepository.UpdateObject(request.GameId, game);
        var prevBoard = JsonHelper.DeserializeWithType<Board>(prevBoardStr);

        return new TurnGameResult
        {
            PirateChanges = _drawService.GetPirateChanges(game.Board, prevBoard),
            Changes = _drawService.GetTileChanges(game.Board, prevBoard),
            Statistics = _drawService.GetStatistics(game),
            TeamScores = game.Board.Teams.Select(team => new TeamScore(team)).ToList(),
            Moves = game.CurrentPlayer is HumanPlayer
                ? _drawService.GetAvailableMoves(game)
                : []
        };
    }
}