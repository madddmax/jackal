using System.Security.Claims;
using JackalWebHost2.Models;
using Microsoft.AspNetCore.Authentication;

namespace JackalWebHost2.Infrastructure.Auth;

public static class FastAuthCookieHelper
{
    public static bool TryExtractUserId(ClaimsPrincipal? claimsPrincipal, out long userId)
    {
        var userClaim = claimsPrincipal?.FindFirst(AuthDefaults.FastAuthClaim);
        return long.TryParse(userClaim?.Value, out userId);
    }

    public static async Task SignInUser(HttpContext httpContext, User user)
    {
        var identity = new ClaimsIdentity(AuthDefaults.FastAuthScheme);
        identity.AddClaim(new Claim(AuthDefaults.FastAuthClaim, user.Id.ToString()));
        
        var principal = new ClaimsPrincipal();
        principal.AddIdentity(identity);
        
        await httpContext.SignInAsync(AuthDefaults.FastAuthScheme, principal);
    }
    
    public static Task SignOutUser(HttpContext httpContext)
    {
        return httpContext.SignOutAsync(AuthDefaults.FastAuthScheme);
    }
}