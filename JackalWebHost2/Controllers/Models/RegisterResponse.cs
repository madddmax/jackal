namespace JackalWebHost2.Controllers.Models;

public class RegisterResponse
{
    public UserModel? User { get; set; }

    public string? Token { get; set; }
}