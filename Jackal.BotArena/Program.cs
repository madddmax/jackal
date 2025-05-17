using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Jackal.Core.MapGenerator;
using Jackal.Core.Players;

namespace Jackal.BotArena;

/// <summary>
/// Консольное приложение для оценки силы игроков ботов
/// </summary>
internal static class Program
{
    /// <summary>
    /// Игровая статистика
    /// </summary>
    private class GameStat
    {
        /// <summary>
        /// Количество побед, когда в конце игры золота оказалось больше.
        /// В случае равенства по золоту, победа присуждается обеим командам.
        /// </summary>
        public int TotalWin { get; set; }
        
        /// <summary>
        /// Суммарное количество добытых монет за все игры
        /// </summary>
        public int TotalCoins { get; set; }
        
        /// <summary>
        /// Количество проведенных игр
        /// </summary>
        public int GamesCount { get; set; }

        /// <summary>
        /// Среднее количество побед за все игры
        /// </summary>
        public double AverageWin => (double)TotalWin / GamesCount;
        
        /// <summary>
        /// Среднее количество добытых монет за все игры
        /// </summary>
        public double AverageCoins => (double)TotalCoins / GamesCount;
    }

    /// <summary>
    /// Статистика по каждому игроку боту
    /// </summary>
    private static readonly Dictionary<string, GameStat> BotStat = new();
    
    private static void Main(string[] args)
    {
        IPlayer[][] combinationOfPlayers =
        [
            [
                new EasyPlayer(),
                new RandomPlayer()
            ],
            [
                new RandomPlayer(),
                new EasyPlayer(),
            ],
            [
                new EasyPlayer(),
                new OakioPlayer(),
            ],
            [
                new OakioPlayer(),
                new EasyPlayer()
            ]
        ];

        int totalGamesCount = 100;
        int gameNumber = 0;
        
        while (gameNumber < totalGamesCount)
        {
            var mapId = new Random().Next();
            var mapSize = 13;
            var randomMap = new RandomMapGenerator(mapId, mapSize);
            
            foreach (var players in combinationOfPlayers)
            {
                var gameRequest = new GameRequest(mapSize, randomMap, players);
                var game = new Game(gameRequest);
                
                while (game.IsGameOver == false)
                {
                    game.Turn();
                }

                CalcStat(game);
                gameNumber++;
            }
        }
        
        ShowStat(gameNumber);
    }

    private static void CalcStat(Game game)
    {
        var maxCoins = game.Board.Teams.Max(x => x.Coins);
        foreach (var team in game.Board.Teams)
        {
            if (!BotStat.TryGetValue(team.Name, out var stat))
            {
                stat = new GameStat();
                BotStat.Add(team.Name, stat);
            }
            stat.TotalWin += team.Coins == maxCoins ? 1 : 0;
            stat.TotalCoins += team.Coins;
            stat.GamesCount += 1;
        }
    }

    private static void ShowStat(int gameNumber)
    {
        Console.WriteLine($"Total games count = {gameNumber}");
        var orderedBotStat = BotStat.OrderByDescending(p => p.Value.AverageWin);
        foreach (var (botName, gameStat) in orderedBotStat)
        {
            Console.WriteLine(
                $"Bot name = {botName} | " +
                $"Games count = {gameStat.GamesCount} | " +
                $"Average win = {gameStat.AverageWin:P} | " +
                $"Average coins = {gameStat.AverageCoins:F} | " +
                $"Total win = {gameStat.TotalWin} | " +
                $"Total coins = {gameStat.TotalCoins}"
            );
        }
    }
}