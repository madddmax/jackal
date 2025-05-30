using JackalWebHost2.Models;

namespace JackalWebHost2.Controllers.Models.Game
{
    public class NetGameResponse
    {
        public long Id { get; set; }
        public long? GameId { get; set; }
        public GameSettings Settings { get; set; }
        public HashSet<long> Viewers { get; set; }
    }
}
