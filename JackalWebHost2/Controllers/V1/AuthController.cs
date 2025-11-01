using JackalWebHost2.Controllers.Models;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Exceptions;
using JackalWebHost2.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JackalWebHost2.Controllers.V1;

[FastAuth]
[Route("/api/v1/auth")]
public class AuthController(IUserRepository userRepository) : Controller
{
    /// <summary>
    /// Зарегистрироваться (без авторизации)
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<RegisterResponse> Register([FromBody] RegisterRequest request, CancellationToken token)
    {
        if (HttpContext.User.Identity?.IsAuthenticated == true)
        {
            throw new UserIsAlreadyLoggedInException();
        }

        var user = await userRepository.GetUser(request.Login, token)
                   ?? await userRepository.CreateUser(request.Login, token);
        
        return new RegisterResponse
        {
            User = new UserModel
            {
                Id = user.Id,
                Login = user.Login,
                Rank = user.Rank
            },
            Token = await FastAuthJwtBearerHelper.SignInUser(HttpContext, user)
        };
    }

    /// <summary>
    /// Проверить состояние авторизации
    /// </summary>
    [AllowAnonymous]
    [HttpPost("check")]
    public async Task<CheckResponse> Check([FromBody] CheckRequest request, CancellationToken token)
    {
        if (HttpContext.User.Identity?.IsAuthenticated == false)
        {
            return new CheckResponse();
        }

        var tokenUser = FastAuthJwtBearerHelper.ExtractUser(HttpContext.User);
        var user = await userRepository.GetUser(tokenUser.Id, token);
        if(user == null)
        {
            return new CheckResponse();
        }
        
        return new CheckResponse
        {
            User = new UserModel
            {
                Id = user.Id,
                Login = user.Login,
                Rank = user.Rank
            }
        };
    }

    /// <summary>
    /// Выйти
    /// </summary>
    [HttpPost("logout")]
    public async Task<LogoutResponse> Logout([FromBody] LogoutRequest request, CancellationToken token)
    {
        // TODO: уведомляем сервер о разлогине
        var user = FastAuthJwtBearerHelper.ExtractUser(HttpContext.User);
        return new LogoutResponse();
    }
}