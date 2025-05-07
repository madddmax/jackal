namespace JackalWebHost2.Data.Entities
{
    public class GameCacheEntry
    {
        public long GameId { get; init; }

        [Obsolete("Заменить на Creator")]
        public long CreatorId { get; init; }

        public GameCacheEntryCreator Creator { get; init; } = new GameCacheEntryCreator{ Name = "Неизвестно" };

        public long TimeStamp { get; set; }
    }


    public class GameCacheEntryCreator
    {
        public long Id { get; init; }

        public string? Name { get; init; }
    }
}
