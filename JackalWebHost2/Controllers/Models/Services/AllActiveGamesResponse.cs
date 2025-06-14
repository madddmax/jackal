using JackalWebHost2.Data.Entities;

namespace JackalWebHost2.Controllers.Models.Services
{
    public class AllActiveGamesResponse
    {
        public IList<ActiveGameInfo>? GamesEntries { get; set; }
    }

    public class ActiveGameInfo
    {
        public long GameId { get; set; }

        public CacheEntryUser? Creator { get; set; }

        public CacheEntryUser[]? Players { get; set; }

        public long TimeStamp { get; set; }
    }
}
