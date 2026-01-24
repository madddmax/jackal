using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jackal.Core;
using Jackal.Core.MapGenerator;
using Jackal.Core.MapGenerator.TilesPack;
using Jackal.Core.Players;

namespace Jackal.BotArena;

/// <summary>
/// Консольное приложение для оценки силы игроков ботов
/// </summary>
internal static class Program
{
    /// <summary>
    /// Количество запускаемых игр
    /// </summary>
    private const int ArenaGamesCount = 500;

    /// <summary>
    /// Размер карты
    /// </summary>
    private const int MapSize = 11;
    
    /// <summary>
    /// Комбинации игроков ботов и их позиций (зависит от порядка)
    /// </summary>
    private static readonly IPlayer[][] CombinationOfPlayers =
    [
        [
            new VeryEasyPlayer(),
            new EasyPlayer()
        ],
        [
            new EasyPlayer(),
            new VeryEasyPlayer()
        ]
    ];
    
    /// <summary>
    /// Статистика по каждому игроку боту
    /// </summary>
    private static readonly ConcurrentDictionary<string, GamePlayerStat> BotStat = new();

    /// <summary>
    /// Общее количество ходов
    /// </summary>
    private static long _totalTurns;
    
    private static void Main()
    {
        int gameNumber = 0;

        var timeElapsed = StopwatchMeter.GetElapsed(() =>
        {
            for (int index = 1; gameNumber < ArenaGamesCount; index++)
            {
                var mapId = index;
                
                Parallel.ForEach(CombinationOfPlayers, (players, state) =>
                {
                    var randomMap = new RandomMapGenerator(mapId, MapSize, TilesPackFactory.Extended);
                    var gameRequest = new GameRequest(MapSize, randomMap, players);
                    var game = new Game(gameRequest);

                    while (game.IsGameOver == false)
                    {
                        game.Turn();
                        Interlocked.Increment(ref _totalTurns);
                    }

                    CalcStat(game);

                    gameNumber++;
                    if (gameNumber == ArenaGamesCount)
                    {
                        state.Break();
                    }
                });
            }
        });

        ShowStat(gameNumber, timeElapsed);
    }

    private static void CalcStat(Game game)
    {
        var maxCoins = game.Board.Teams.Max(x => x.Coins);
        foreach (var team in game.Board.Teams)
        {
            if (!BotStat.TryGetValue(team.PlayerName, out var stat))
            {
                stat = new GamePlayerStat { PlayerName = team.PlayerName };
                BotStat.TryAdd(team.PlayerName, stat);
            }
            stat.TotalWin += team.Coins == maxCoins ? 1 : 0;
            stat.TotalLose += team.Coins != maxCoins ? 1 : 0;
            stat.TotalCoins += team.Coins;
        }
    }

    private static void ShowStat(int gamesCount, TimeSpan timeElapsed)
    {
        Console.WriteLine($"Arena games count = {gamesCount} | Total turns {_totalTurns} | Time elapsed {timeElapsed}");
        var orderedBotStat = BotStat.OrderByDescending(p => p.Value.WinPercent);
        foreach (var (_, gamePlayerStat) in orderedBotStat)
        {
            Console.WriteLine(
                $"Player name = {gamePlayerStat.PlayerName} | " +
                $"Win percent = {gamePlayerStat.WinPercent:F}% | " +
                $"Total win = {gamePlayerStat.TotalWin} | " +
                $"Total lose = {gamePlayerStat.TotalLose} | " +
                $"Average coins = {gamePlayerStat.AverageCoins:F} | " +
                $"Total coins = {gamePlayerStat.TotalCoins}"
            );
        }
    }
}