using Jackal.Core.Actions;
using Jackal.Core.Domain;

namespace Jackal.Core;

/// <summary>
/// Возможный ход
/// </summary>
public class AvailableMove(TilePosition from, TilePosition to, params IGameAction[] actions)
{
    /// <summary>
    /// Список действий, которые надо выполнить
    /// </summary>
    public readonly GameActionList ActionList = new(actions);
    
    /// <summary>
    /// Откуда идем
    /// </summary>
    public readonly TilePosition From = from;
    
    /// <summary>
    /// Куда идем
    /// </summary>
    public readonly TilePosition To = to;
    
    /// <summary>
    /// Предыдущая позиция - требуется для определения с какой кдетки идем на закрытую клетку,
    /// это важно если открываем лед или крокодила, т.к. дальше эти клетки используют prev
    /// </summary>
    public Position? Prev;
    
    /// <summary>
    /// Тип хода
    /// </summary>
    public MoveType MoveType = MoveType.Usual;

    /// <summary>
    /// Ход
    /// </summary>
    public Move ToMove => new(From, To, Prev, MoveType);
}