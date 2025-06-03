using System;

namespace Jackal.Core.Players;

/// <summary>
/// Интерфейс игрока человека
/// </summary>
public interface IHumanPlayer
{
    /// <summary>
    /// ИД пользователя
    /// </summary>
    long UserId { get; }
    
    /// <summary>
    /// Имя игрока
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Выбор хода для человека
    /// </summary>
    /// <param name="moveNum">Номер хода из доступных ходов</param>
    /// <param name="pirateId">Пират который ходит, можно не передавать</param>
    void SetMove(int moveNum, Guid? pirateId);
}