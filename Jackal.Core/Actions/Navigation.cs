namespace Jackal.Actions
{
    /*
    class Navigation : IGameAction
    {
        private readonly Ship _ship;
        private readonly Position _to;

        public Navigation(Ship ship, Position to)
        {
            _ship = ship;
            _to = to;
        }

        public GameActionResult Act(Game game)
        {
            Board board = game.Board;
            Tile shipTile = board.Map[_ship.Position];
            Tile toTile = board.Map[_to];
            _ship.Position = _to;

            foreach (var pirate in shipTile.Pirates)
            {
                pirate.Position = new TilePosition(_to);
                toTile.Pirates.Add(pirate);
            }
            shipTile.Pirates.Clear();

            return GameActionResult.Live;
        }
    }
    */ 
}