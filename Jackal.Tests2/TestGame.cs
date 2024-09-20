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
    /// <param name="piratesPerPlayer">Пиратов в команде, по умолчанию 1</param>
    public TestGame (IMapGenerator generator, int mapSize = 5, int piratesPerPlayer = 1)
    {
        IPlayer[] players = [new WebHumanPlayer()];
        var board = new Board(players, generator, mapSize, piratesPerPlayer);
        _testGame = new Game(players, board);
    }

    public List<Move> GetAvailableMoves()
    {
        return _testGame.GetAvailableMoves();
    }
    
    public void Turn()
    {
        _testGame.CurrentPlayer.SetHumanMove(0, null);
        _testGame.Turn();
    }
    
    public void SetMoveAndTurn(int x, int y)
    {
        var position = new TilePosition(x, y);
        var moves = _testGame.GetAvailableMoves();
        var moveNum = moves.FindIndex(a => a.To == position);
        
        _testGame.CurrentPlayer.SetHumanMove(moveNum, null);
        _testGame.Turn();
    }
    
    public void SetMoveAndTurn(TilePosition from, TilePosition to)
    {
        var moves = _testGame.GetAvailableMoves();
        var moveNum = moves.FindIndex(a => a.From == from && a.To == to);
        
        _testGame.CurrentPlayer.SetHumanMove(moveNum, null);
        _testGame.Turn();
    }
}