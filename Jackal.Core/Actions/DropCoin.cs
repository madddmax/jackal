namespace Jackal.Core.Actions
{
    /*
    class DropCoin : IGameAction
    {
        private readonly Pirate _pirate;

        public DropCoin(Pirate pirate)
        {
            _pirate = pirate;
        }

        public void Act(Game game)
        {
            Board board = game.Board;
            if (_pirate.Coins == 0)
            {
                return;
            }

            Position position = _pirate.Position;
            Tile tile = board.Map[position.X, position.Y];
            tile.Coins += _pirate.Coins;
            _pirate.Coins = 0;
        }
    }
     */ 
}