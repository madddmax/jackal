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

    /// <summary>
    /// Конец игры
    /// </summary>
    public bool IsGameOver => _testGame.IsGameOver;
    
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="generator">Генератор карты</param>
    /// <param name="mapSize">Размер карты вместе с морем, по умолчанию минимальный 5x5 (поле из 5 клеток)</param>
    public TestGame (IMapGenerator generator, int mapSize = 5)
    {
        IPlayer[] players = [new WebHumanPlayer()];
        var board = new Board(players, generator, mapSize, PiratesPerPlayer);
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