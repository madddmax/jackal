using System;

namespace Jackal.Core.Players
{
    public class RandomPlayer : IPlayer
    {
        private Random Rnd;

        public void OnNewGame()
        {
            Rnd = new Random(42);
        }

        public void SetHumanMove(int moveNum)
        {
            throw new NotImplementedException();
        }

        public int OnMove(GameState gameState)
        {
            return Rnd.Next(gameState.AvailableMoves.Length);
        }
    }
}