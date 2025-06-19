using Jackal.Core;
using JackalWebHost2.Models.Auth;

namespace JackalWebHost2.Models
{
    public class NetGameSettings : ICompletable
    {
        public NetGameSettings()
        {
        }

        public NetGameSettings(User creator)
        {
            Users.Add(creator);
            CreatorId = creator.Id;
        }

        public long Id { get; set; }

        public long? GameId { get; set; }
        public bool IsCompleted => GameId.HasValue;

        public long CreatorId { get; set; }

        public HashSet<User> Users { get; } = new (new UserComparer());

        public GameSettings Settings { get; set; } = new();
    }
}
