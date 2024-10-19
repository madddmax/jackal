namespace JackalWebHost2.Controllers.Models;

public class CheckResponse
{
    public UserModel? User { get; set; }

    public bool IsAuthorised => User != null;
}