namespace Jackal.Core.Players
{
    public abstract class BlankPlayer : IPlayer
    {
        private SmartPlayer player;

        public virtual void OnNewGame()
        {
            player = new SmartPlayer();
            player.OnNewGame();
        }

        public void SetHumanMove(int moveNum)
        {
            throw new System.NotImplementedException();
        }

        public virtual int OnMove(GameState gameState)
        {
            return player.OnMove(gameState);
        }
    }
}