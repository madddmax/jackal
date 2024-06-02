namespace Jackal.Core.Players
{
    public class WebHumanPlayer : IPlayer
    {
        private int _move;

        public void OnNewGame()
        {
            _move = 0;
        }

        public void SetHumanMove(int moveNum)
        {
            _move = moveNum;
        }

        public int OnMove(GameState gameState)
        {
            return _move;
        }
    }
}
