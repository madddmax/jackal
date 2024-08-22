using System.Collections.Generic;
using Jackal.Core;
using Jackal.Core.Players;

namespace Jackal.Tests2;

/// <summary>
/// Класс обертка для тестирования игры,
/// чтобы не плодить зависимости на Game
/// </summary>
public class TestGame
{
    private readonly Game _testGame;
    
    public TestGame (IMapGenerator generator)
    {
        IPlayer[] players = [new WebHumanPlayer()];
        var board = new Board(players, generator, 5, 1);
        _testGame = new Game(players, board);
    }

    public List<Move> GetAvailableMoves()
    {
        return _testGame.GetAvailableMoves();
    }
    
    public void Turn()
    {
        _testGame.Turn();
    }
}