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

    public User GetUser()
    {
        return TryGetUser(out var user) ? user! : throw new NotSupportedException("User expected to be logged in");
    }

    public void SetUser(User user)
    {
        _user = user;
    }
}