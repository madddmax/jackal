namespace Jackal
{
    public interface IPlayer
    {
        void OnNewGame();

        /// <summary>
        /// Насильный выбор хода, для HumanPlayer
        /// </summary>
        void SetHumanMove(int moveNum);

        int OnMove(GameState gameState);
    }
}