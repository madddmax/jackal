using Microsoft.AspNetCore.Authorization;

namespace JackalWebHost2.Infrastructure.Auth;

public class FastAuthAttribute : AuthorizeAttribute
{
    public FastAuthAttribute()
    {
        Policy = AuthDefaults.FastAuthPolicy;
    }
}