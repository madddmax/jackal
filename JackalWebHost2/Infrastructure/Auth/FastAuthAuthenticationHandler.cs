using System.Text.Encodings.Web;
using JackalWebHost2.Data.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace JackalWebHost2.Infrastructure.Auth;

public class FastAuthAuthenticationHandler : CookieAuthenticationHandler
{
    private readonly IUserAuthProvider _userAuthProvider;
    private readonly IUserRepository _userRepository;

    public FastAuthAuthenticationHandler(
        IOptionsMonitor<CookieAuthenticationOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder, 
        IUserAuthProvider userAuthProvider, 
        IUserRepository userRepository) : 
        base(options, logger, encoder)
    {
        _userAuthProvider = userAuthProvider;
        _userRepository = userRepository;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authenticateResult = await base.HandleAuthenticateAsync();
        if (!authenticateResult.Succeeded)
        {
            return authenticateResult;
        }

        if (await ValidateAndSetUser(authenticateResult))
        {
            return authenticateResult;
        }
        
        return AuthenticateResult.Fail("User is not found");
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Context.Response.StatusCode = 401;
        return Task.CompletedTask;
    }

    protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Context.Response.StatusCode = 403;
        return Task.CompletedTask;
    }

    private async Task<bool> ValidateAndSetUser(AuthenticateResult authenticateResult)
    {
        if (!FastAuthCookieHelper.TryExtractUserId(authenticateResult.Principal, out var userId))
        {
            return false;
        }

        var user = await _userRepository.GetUser(userId, CancellationToken.None);
        if (user == null)
        {
            Logger.LogWarning("User with id {UserId} is not found", userId);
            return false;
        }

        _userAuthProvider.SetUser(user);
        return true;
    }
}