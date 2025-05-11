using System;

namespace Jackal.Core.Players;

public class RandomPlayer : IPlayer
{
    private Random Rnd;

    public long UserId => 0;
    
    public void OnNewGame()
    {
        Rnd = new Random(42);
    }

    public (int moveNum, Guid? pirateId) OnMove(GameState gameState)
    {
        return (Rnd.Next(gameState.AvailableMoves.Length), null);
    }
}