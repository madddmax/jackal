namespace JackalWebHost2.Controllers.Models;

public class UserModel
{
    public long Id { get; set; }
    
    public required string Login { get; set; }
    
    public required string Rank { get; set; }
}