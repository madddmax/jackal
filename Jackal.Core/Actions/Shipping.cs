namespace Jackal.Core.Actions
{
    /*
    class Shipping : IGameAction
    {
        private readonly Pirate _pirate;
        private readonly Ship _ship;

        public Shipping(Pirate pirate, Ship ship)
        {
            _pirate = pirate;
            _ship = ship;
        }

        public GameActionResult Act(Game game)
        {
            var position = _pirate.Position;
            Board board = game.Board;
            TileLevel tile = board.Map[position];
            tile.Pirates.Remove(_pirate);

            _pirate.Position = new TilePosition(_ship.Position);
            board.Map[_ship.Position].Pirates.Add(_pirate);
            //var coins = _pirate.Coins;
            //_ship.Coins += coins;
            //_pirate.Coins = 0;

            //game.Scores[_pirate.TeamId] += coins;
            //game.CoinsLeft -= coins;

            return GameActionResult.Live;
        }
    }
    */ 
}