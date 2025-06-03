using System;

namespace Jackal.Core.Players;

/// <summary>
/// Игрок человек - выбирает ход через Web UI
/// </summary>
/// <param name="userId">ИД пользователя</param>
/// <param name="name">Имя игрока</param>
public class HumanPlayer(long userId, string name) : IHumanPlayer, IPlayer
{
    private int _moveNum;
    private Guid? _pirateId;

    public long UserId { get; } = userId;

    public string Name { get; } = name;

    public void OnNewGame()
    {
        _moveNum = 0;
        _pirateId = null;
    }

    public void SetMove(int moveNum, Guid? pirateId)
    {
        _moveNum = moveNum;
        _pirateId = pirateId;
    }

    public (int moveNum, Guid? pirateId) OnMove(GameState gameState)
    {
        return (_moveNum, _pirateId);
    }
}