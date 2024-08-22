using System;
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
    /// <summary>
    /// Минимальный размер карты 5x5, поле 5 клеток
    /// </summary>
    private const int MapSize = 5;

    /// <summary>
    /// Один пират на команду
    /// </summary>
    private const int PiratesPerPlayer = 1;

    /// <summary>
    /// Тестовая игра
    /// </summary>
    private readonly Game _testGame;
    
    /// <summary>
    /// Игровое поле
    /// </summary>
    public Board Board => _testGame.Board;
    
    /// <summary>
    /// Текущий ход - определяет какая команда ходит
    /// </summary>
    public int TurnNo => _testGame.TurnNo;
    
    public TestGame (IMapGenerator generator)
    {
        IPlayer[] players = [new WebHumanPlayer()];
        var board = new Board(players, generator, MapSize, PiratesPerPlayer);
        _testGame = new Game(players, board);
    }

    public List<Move> GetAvailableMoves()
    {
        return _testGame.GetAvailableMoves();
    }

    public void SetMove(int moveNum, Guid? pirateId = null)
    {
        _testGame.CurrentPlayer.SetHumanMove(moveNum, pirateId);
    }
    
    public void Turn()
    {
        _testGame.Turn();
    }
}