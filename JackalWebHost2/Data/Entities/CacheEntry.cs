namespace JackalWebHost2.Data.Entities
{
    public class CacheEntry
    {
        public long ObjectId { get; init; }

        public CacheEntryCreator Creator { get; init; } = new CacheEntryCreator{ Name = "Неизвестно" };

        public long TimeStamp { get; set; }
    }


    public class CacheEntryCreator
    {
        public long Id { get; init; }

        public string? Name { get; init; }
    }
}
