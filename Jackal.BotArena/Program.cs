using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Jackal.Core.MapGenerator;
using Jackal.Core.Players;

namespace Jackal.BotArena;

internal static class Program
{
    private class ArenaStat
    {
        public int TotalWin { get; set; }
        public int TotalCoins { get; set; }
    }

    private static readonly Dictionary<string, ArenaStat> BotStat = new();
    
    private static void Main(string[] args)
    {
        IPlayer[][] combinationOfPlayers =
        [
            [
                new RandomPlayer(),
                new EasyBotPlayer()
            ],
            [
                new EasyBotPlayer(),
                new RandomPlayer()
            ]
        ];

        int gamesCount = 100;
        int gameNumber = 0;
        
        while (gameNumber < gamesCount)
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
                stat = new ArenaStat();
                BotStat.Add(team.Name, stat);
            }
            stat.TotalWin += team.Coins == maxCoins ? 1 : 0;
            stat.TotalCoins += team.Coins;
        }
    }

    private static void ShowStat(int gameNumber)
    {
        Console.WriteLine($"Games count = {gameNumber}");
        var orderedBotStat = BotStat.OrderByDescending(p => p.Value.TotalWin);
        foreach (var (botName, arenaStat) in orderedBotStat)
        {
            Console.WriteLine(
                $"Bot name = {botName} | Total win = {arenaStat.TotalWin} | Total coins = {arenaStat.TotalCoins}"
            );
        }
    }
}