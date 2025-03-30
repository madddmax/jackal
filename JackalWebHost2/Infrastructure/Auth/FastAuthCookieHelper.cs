using System.Security.Claims;
using JackalWebHost2.Models;
using Microsoft.AspNetCore.Authentication;

namespace JackalWebHost2.Infrastructure.Auth;

public static class FastAuthCookieHelper
{
    public static User ExtractUser(ClaimsPrincipal? user)
    {
        var idClaim = user?.FindFirst(AuthDefaults.FastAuthUserId);
        var loginClaim = user?.FindFirst(AuthDefaults.FastAuthLogin);
        
        return new User
        {
            Id = long.TryParse(idClaim?.Value, out var id) ? id : 0,
            Login = loginClaim?.Value ?? "dark incognito"
        };
    }
    
    public static async Task SignInUser(HttpContext httpContext, User user)
    {
        var identity = new ClaimsIdentity(AuthDefaults.FastAuthScheme);
        identity.AddClaim(new Claim(AuthDefaults.FastAuthUserId, user.Id.ToString()));
        identity.AddClaim(new Claim(AuthDefaults.FastAuthLogin, user.Login));
        
        var principal = new ClaimsPrincipal();
        principal.AddIdentity(identity);
        
        await httpContext.SignInAsync(AuthDefaults.FastAuthScheme, principal);
    }
    
    public static Task SignOutUser(HttpContext httpContext)
    {
        return httpContext.SignOutAsync(AuthDefaults.FastAuthScheme);
    }
}