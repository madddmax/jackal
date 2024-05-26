using System;

namespace Jackal.Actions
{
    /*
    class Landing : IGameAction
    {
        private readonly Pirate _pirate;
        private readonly Ship _ship;

        public Landing(Pirate pirate, Ship ship)
        {
            _pirate = pirate;
            _ship = ship;
        }

        public GameActionResult Act(Game game)
        {
            Board board = game.Board;
            Position shipPosition = _ship.Position;

            Tile shipTile = board.Map[shipPosition];
            shipTile.Pirates.Remove(_pirate);

            Position landingPosition = GetLandingPosition(shipPosition);
            _pirate.Position = new TilePosition(landingPosition);

            return GameActionResult.Live;
        }

        private Position GetLandingPosition(Position shipPosition)
        {
            Position landingPosition;
            if (shipPosition.X == 0)
            {
                landingPosition = new Position(shipPosition.X + 1, shipPosition.Y);
            }
            else if (shipPosition.X == (Board.Size - 1))
            {
                landingPosition = new Position(shipPosition.X - 1, shipPosition.Y);
            }
            else if (shipPosition.Y == 0)
            {
                landingPosition = new Position(shipPosition.X, shipPosition.Y + 1);
            }
            else if (shipPosition.Y == (Board.Size - 1))
            {
                landingPosition = new Position(shipPosition.X, shipPosition.Y - 1);
            }
            else
            {
                throw new NotSupportedException();
            }

            return landingPosition;
        }
    }
     */ 
}