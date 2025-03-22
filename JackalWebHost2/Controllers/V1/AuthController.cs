using JackalWebHost2.Controllers.Models;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Exceptions;
using JackalWebHost2.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JackalWebHost2.Controllers.V1;

[FastAuth]
[Route("/api/v1/auth")]
public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IUserAuthProvider _userAuthProvider;

    public AuthController(IUserRepository userRepository, IUserAuthProvider userAuthProvider)
    {
        _userRepository = userRepository;
        _userAuthProvider = userAuthProvider;
    }
    
    /// <summary>
    /// Зарегистрироваться (без авторизации)
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<RegisterResponse> Register([FromBody] RegisterRequest request, CancellationToken token)
    {
        if (_userAuthProvider.TryGetUser(out _))
        {
            throw new UserIsAlreadyLoggedInException();
        }

        var user = await _userRepository.CreateUser(request.Login, token);
        await FastAuthCookieHelper.SignInUser(HttpContext, user);

        return new RegisterResponse
        {
            User = new UserModel
            {
                Id = user.Id,
                Login = user.Login
            }
        };
    }

    /// <summary>
    /// Проверить состояние авторизации
    /// </summary>
    [AllowAnonymous]
    [HttpPost("check")]
    public async Task<CheckResponse> Check([FromBody] CheckRequest request, CancellationToken token)
    {
        if (!_userAuthProvider.TryGetUser(out var user))
        {
            return new CheckResponse();
        }

        return new CheckResponse
        {
            User = new UserModel
            {
                Id = user!.Id,
                Login = user.Login
            }
        };
    }
    
    /// <summary>
    /// Выйти
    /// </summary>
    [HttpPost("logout")]
    public async Task<LogoutResponse> Logout([FromBody] LogoutRequest request, CancellationToken token)
    {
        await FastAuthCookieHelper.SignOutUser(HttpContext);
        return new LogoutResponse();
    }
}