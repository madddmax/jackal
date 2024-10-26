using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core.MapGenerator;

namespace Jackal.Core.GameOrganizer;

public class GameOrganizer
{
    public readonly Results Results = new Results();

    private int _permutationId = -1;
    private int _mapId;

    private readonly int totalPermutationsCount = Utils.Factorial(4);
    List<IdentifiedPlayer> _players = new List<IdentifiedPlayer>();

    public GameOrganizer(int? mapId, IEnumerable<IdentifiedPlayer> players)
    {
        _players.AddRange(players);
        if (_players.Count != 4)
            throw new ArgumentException("players");

        if (mapId.HasValue)
            _mapId = mapId.Value;
        else
            _mapId = new Random().Next();
    }

    List<IEnumerable<string>> _playersSets = new List<IEnumerable<string>>();

    bool WasPlayersSet(IList<IdentifiedPlayer> players)
    {
        foreach (IEnumerable<string> set in _playersSets)
        {
            if (set.SequenceEqual(players.Select(x => x.Id))) return true;
        }
        return false;
    }

    void ClearPlayersSet()
    {
        _playersSets.Clear();
    }

    void AddPlayersSet(IList<IdentifiedPlayer> players)
    {
        _playersSets.Add(players.Select(x => x.Id));
    }

    public void PlayNextGame()
    {
        _permutationId++;

        IdentifiedPlayer[] currentPlayers;
        while (true)
        {
            currentPlayers = Utils.GetPermutation(_permutationId, _players.ToArray()).ToArray();
            if (WasPlayersSet(currentPlayers) == false) break;
            _permutationId++;
            _permutationId %= totalPermutationsCount;
            if (_permutationId == 0)
            {
                _mapId++;
                ClearPlayersSet();
            }
        }

        Results.MapId = _mapId;
        Results.PermutationId = _permutationId;
        Results.GamesCount++;

        var players = currentPlayers.Select(x => x.Player).ToArray();
        var mapSize = 13;
        var randomMap = new RandomMapGenerator(_mapId, mapSize);
        var board = new Board(players, randomMap, mapSize, 3);
        var game = new Game(players, board);

        while (game.IsGameOver == false)
        {
            game.Turn();
        }

        var standing = GetStanding(game);

        for (int playerId = 0; playerId < currentPlayers.Length; playerId++)
        {
            var positionsCount = new double[4];
            for (int position = 1; position <= 4; position++)
            {
                if (standing[playerId] == position)
                {
                    int sameStandingCount = standing.Count(x => x == position);

                    for (int i = 1; i <= sameStandingCount; i++)
                    {
                        positionsCount[position - 1 + i - 1] = 1.0 / sameStandingCount;
                    }
                }
            }
            Results.AddGameResult(currentPlayers[playerId].Id, positionsCount,game.Scores[playerId]);
        }

        AddPlayersSet(currentPlayers);
    }

    /// <summary>
    /// Получаем места игроков
    /// </summary>
    /// <param name="game"></param>
    /// <returns></returns>
    private int[] GetStanding(Game game)
    {
        List<int> gold = new List<int>(4);
        for (int id = 0; id <= 3; id++)
        {
            gold.Add(game.Scores[id]);
        }

        //определяем место
        List<int> list = new List<int>(4);
        for (int id = 0; id <= 3; id++)
        {
            int currentGold = gold[id];
            list.Add(1 + gold.Count(x => x > currentGold));
        }

        return list.ToArray();
    }
}