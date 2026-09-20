namespace JackalWebHost2.Data.Entities;

public class CacheEntry
{
    public long ObjectId { get; init; }

    public CacheEntryUser Creator { get; init; } = new CacheEntryUser { Name = "Неизвестно" };

    public CacheEntryUser[]? Players { get; set; }

    public long TimeStamp { get; set; }
}