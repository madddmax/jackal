using JackalWebHost2.Models;

namespace JackalWebHost2.Controllers.Models.Game
{
    public class NetGameRequest
    {
        public long Id { get; set; }

        public GameSettings Settings { get; set; }
    }
}
