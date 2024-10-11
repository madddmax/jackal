using JackalWebHost2.Controllers.Mappings;
using JackalWebHost2.Controllers.Models;
using JackalWebHost2.Models;
using JackalWebHost2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JackalWebHost2.Controllers.V1;

[AllowAnonymous]
[Route("/api/v1/lobby")]
public class LobbyController : Controller
{
    private readonly ILobbyService _lobbyService;

    public LobbyController(ILobbyService lobbyService)
    {
        _lobbyService = lobbyService;
    }
    
    /// <summary>
    /// Создать лобби с заданными параметрами
    /// </summary>
    [HttpPost("create-lobby")]
    public async Task<CreateLobbyResponse> CreateLobby([FromBody] CreateLobbyRequest request, CancellationToken token)
    {
        // todo Валидировать приходящий null json
        
        // TODO Добавить юзеров
        var user = new User
        {
            Id = Random.Shared.NextInt64(),
            Name = $"name:{Guid.NewGuid():D}"
        };
        
        var result = await _lobbyService.CreateLobby(user, request.Settings, token);

        return new CreateLobbyResponse
        {
            Lobby = result.ToDto()
        };
    }
    
    /// <summary>
    /// Присоединиться к лобби
    /// </summary>
    [HttpPost("join-lobby")]
    public async Task<JoinLobbyResponse> JoinLobby([FromBody] JoinLobbyRequest request, CancellationToken token)
    {
        // TODO Добавить юзеров
        var user = new User
        {
            Id = Random.Shared.NextInt64(),
            Name = $"name:{Guid.NewGuid():D}"
        };
        
        var result = await _lobbyService.JoinLobby(request.LobbyId, user, token);

        return new JoinLobbyResponse
        {
            Lobby = result.ToDto()
        };
    }
}