using System.Linq;

namespace Jackal.Core.Actions
{
    class Respawn : IGameAction
    {
        private readonly Position _to;
        private readonly Team _team;

        public Respawn(Team team, Position to)
        {
            _to = to;
            _team = team;
        }

        public GameActionResult Act(Game game,Pirate pirate)
        {
            Board board = game.Board;
            Tile tile = board.Map[_to];

            Pirate newPirate = new Pirate(_team.Id, new TilePosition(_to));
            _team.Pirates = _team.Pirates.Concat(new[] { newPirate }).ToArray();
            tile.Pirates.Add(newPirate);
            
            return GameActionResult.Live;
        }
    }
}