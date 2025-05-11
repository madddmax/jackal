using System;

namespace Jackal.Core.Players;

public interface IHumanPlayer
{
    /// <summary>
    /// Выбор хода для человека
    /// </summary>
    void SetMove(int moveNum, Guid? pirateId);
}