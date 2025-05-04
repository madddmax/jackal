namespace JackalWebHost2.Data.Entities
{
    public class GameCacheEntry
    {
        public long GameId { get; init; }

        public long CreatorId { get; init; }

        public long TimeStamp { get; set; }
    }
}
