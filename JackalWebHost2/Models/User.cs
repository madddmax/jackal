namespace JackalWebHost2.Models;

public class User
{
    public long Id { get; init; }
    
    public required string Login { get; init; }
    
    public string Rank { get; init; }
}