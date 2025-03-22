namespace JackalWebHost2.Data.Entities;

public class UserEntity
{
    public long Id { get; set; }
    
    public string Login { get; set; }
    
    public DateTime Created { get; set; }
}