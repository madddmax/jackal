using System;

namespace Jackal.Core.Players;

public interface IPlayer
{
    long UserId { get; }
    
    void OnNewGame();

    (int moveNum, Guid? pirateId) OnMove(GameState gameState);
}