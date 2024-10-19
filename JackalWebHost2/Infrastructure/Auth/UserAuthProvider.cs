using JackalWebHost2.Models;

namespace JackalWebHost2.Infrastructure.Auth;

public class UserAuthProvider : IUserAuthProvider
{
    private User? _user;
    
    public bool TryGetUser(out User? user)
    {
        user = _user;
        return user != null;
    }

    public void SetUser(User user)
    {
        _user = user;
    }
}