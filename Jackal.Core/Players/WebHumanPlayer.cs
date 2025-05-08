using System;

namespace Jackal.Core.Players;

public class WebHumanPlayer(long userId) : IPlayer
{
    private int _moveNum;
    private Guid? _pirateId;

    public long UserId { get; } = userId;

    public void OnNewGame()
    {
        _moveNum = 0;
        _pirateId = null;
    }

    public void SetHumanMove(int moveNum, Guid? pirateId)
    {
        _moveNum = moveNum;
        _pirateId = pirateId;
    }

    public (int moveNum, Guid? pirateId) OnMove(GameState gameState)
    {
        return (_moveNum, _pirateId);
    }
}