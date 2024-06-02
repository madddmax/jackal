
namespace Jackal.Core.Actions
{
    /*
    class Explore : IGameAction
    {
        private readonly Position _position;
        private readonly Pirate _pirate;
        private readonly Direction _direction;

        public Explore(Position position, Pirate pirate,Direction direction)
        {
            _position = position;
            _pirate = pirate;
            _direction = direction;
        }

        public GameActionResult Act(Game game)
        {
            Board board = game.Board;
            var newTile = board.Generator.GetNext(_position);
            board.Map[_position] = newTile;

            game.LastActionTurnNo = game.TurnNo;

            if (newTile.Type.RequreImmediateMove())
            {
                var targets = game.Board.GetAllAvaliableMoves(_pirate.TeamId, new TilePosition(_position), new List<CheckedPosition>(), _direction);
                if (targets.Count == 0)
                {
                    game.KillPirate(_pirate);
                    return GameActionResult.Die;
                }
                else //мы попали в клетку, где должны сделать ещё свой выбор
                {
                    game.NeedSubTurnPirate = _pirate;
                    game.PreviosSubTurnDirection = _direction;
                }
            }
            return GameActionResult.Live;
        }
    }
    */ 
}