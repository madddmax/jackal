using System;

namespace Jackal.Core.Players
{
    public class WebHumanPlayer : IPlayer
    {
        private int _moveNum;
        private Guid? _pirateId;

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
}
