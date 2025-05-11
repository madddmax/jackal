using System;

namespace Jackal.Core.Players;

/// <summary>
/// Игрок рэндом - выбирает ход случайным образом
/// </summary>
public class RandomPlayer : IPlayer
{
    private Random _rnd = new();
    
    public void OnNewGame()
    {
        _rnd = new Random(42);
    }

    public (int moveNum, Guid? pirateId) OnMove(GameState gameState)
    {
        return (_rnd.Next(gameState.AvailableMoves.Length), null);
    }
}