using System;

namespace Jackal.Core.Players;

/// <summary>
/// Игрок бот реализованный фанатом игры oakio
/// </summary>
public class OakioPlayer : IPlayer
{
    public void OnNewGame()
    {
    }

    public (int moveNum, Guid? pirateId) OnMove(GameState gameState)
    {
        // выбирает первый ход из доступных ходов
        return new ValueTuple<int, Guid?>(0, null);
    }
}