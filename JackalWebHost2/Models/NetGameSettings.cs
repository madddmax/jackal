using Jackal.Core;

namespace JackalWebHost2.Models
{
    public class NetGameSettings : ICompletable
    {
        public long Id { get; set; }

        public long? GameId { get; set; }
        public bool IsCompleted => GameId.HasValue;

        public long CreatorId { get; set; }

        public HashSet<long> Users { get; set; } = new();

        public GameSettings Settings { get; set; } = new();
    }
}
