using System;

namespace Jackal.Core.Players;

/// <summary>
/// Интерфейс игрока
/// </summary>
public interface IPlayer
{
    /// <summary>
    /// Инициализация новой игры
    /// </summary>
    void OnNewGame();

    /// <summary>
    /// Игровой ход
    /// </summary>
    /// <param name="gameState">Состояние игры</param>
    /// <returns>
    /// moveNum - номер хода из доступных ходов
    /// pirateId - пират который ходит, можно не передавать
    /// </returns>
    (int moveNum, Guid? pirateId) OnMove(GameState gameState);
}