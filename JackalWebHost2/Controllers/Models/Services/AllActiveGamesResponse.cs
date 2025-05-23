using JackalWebHost2.Data.Entities;

namespace JackalWebHost2.Controllers.Models.Services
{
    public class AllActiveGamesResponse
    {
        public IList<GameCacheEntry>? GamesEntries { get; set; }
    }
}
