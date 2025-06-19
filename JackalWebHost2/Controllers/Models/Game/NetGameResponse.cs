using System.Diagnostics.CodeAnalysis;
using JackalWebHost2.Models;

namespace JackalWebHost2.Controllers.Models.Game
{
    public class NetGameResponse
    {
        public long Id { get; set; }
        public long CreatorId { get; set; }
        public long? GameId { get; set; }

        [NotNull]
        public GameSettings? Settings { get; set; }

        [NotNull]
        public long[]? Viewers { get; set; }

        [NotNull]
        public HashSet<User>? Users { get; set; }
    }
}
