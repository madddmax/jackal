namespace JackalWebHost2.Data.Entities;

public class CacheEntryEntity
{
    public long ObjectId { get; set; }

    public string Payload { get; set; } = default!; // JSON сериализованный T

    // Информация о создателе (CacheEntryUser)
    public long CreatorId { get; set; }
    public string? CreatorName { get; set; }

    // JSON массив CacheEntryUser
    public string? PlayersJson { get; set; }

    public long TimeStamp { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public DateTime Created { get; set; }
    
    public DateTime Updated { get; set; }
}