using JackalWebHost2.Controllers.Mappings;
using JackalWebHost2.Controllers.Models.Lobby;
using JackalWebHost2.Infrastructure.Auth;
using JackalWebHost2.Services;
using Microsoft.AspNetCore.Mvc;

namespace JackalWebHost2.Controllers.V1;

[FastAuth]
[Route("/api/v1/lobby")]
public class LobbyController : Controller
{
    private readonly ILobbyService _lobbyService;
    private readonly IUserAuthProvider _userAuthProvider;

    public LobbyController(ILobbyService lobbyService, IUserAuthProvider userAuthProvider)
    {
        _lobbyService = lobbyService;
        _userAuthProvider = userAuthProvider;
    }
    
    /// <summary>
    /// Создать лобби с заданными параметрами
    /// </summary>
    [HttpPost("create-lobby")]
    public async Task<CreateLobbyResponse> CreateLobby([FromBody] CreateLobbyRequest request, CancellationToken token)
    {
        // todo Валидировать приходящий null json по всем контроллерам
        
        var user = _userAuthProvider.GetUser();
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
        var user = _userAuthProvider.GetUser();
        var result = await _lobbyService.JoinLobby(request.LobbyId, user, token);

        return new JoinLobbyResponse
        {
            Lobby = result.ToDto()
        };
    }
    
    /// <summary>
    /// Покинуть лобби
    /// </summary>
    [HttpPost("leave-lobby")]
    public async Task<LeaveLobbyResponse> LeaveLobby([FromBody] LeaveLobbyRequest request, CancellationToken token)
    {
        var user = _userAuthProvider.GetUser();
        await _lobbyService.LeaveLobby(user, token);
        return new LeaveLobbyResponse();
    }
    
    /// <summary>
    /// Получить информацию о лобби
    /// </summary>
    [HttpPost("get-lobby")]
    public async Task<GetLobbyResponse> GetLobby([FromBody] GetLobbyRequest request, CancellationToken token)
    {
        var user = _userAuthProvider.GetUser();
        var result = await _lobbyService.GetLobbyInfo(request.LobbyId, user, token);

        return new GetLobbyResponse
        {
            Lobby = result.ToDto()
        };
    }
    
    /// <summary>
    /// Выгнать игрока из лобби
    /// </summary>
    [HttpPost("kick-from-lobby")]
    public async Task<KickFromLobbyResponse> KickPlayer([FromBody] KickFromLobbyRequest request, CancellationToken token)
    {
        var user = _userAuthProvider.GetUser();
        await _lobbyService.KickPlayer(request.LobbyId, user, request.TargetUserId, token);
        return new KickFromLobbyResponse();
    }
    
    /// <summary>
    /// Сменить команду для игрока
    /// </summary>
    [HttpPost("assign-team")]
    public async Task<AssignTeamResponse> AssignTeam([FromBody] AssignTeamRequest request, CancellationToken token)
    {
        var user = _userAuthProvider.GetUser();
        await _lobbyService.AssignTeam(request.LobbyId, user, request.UserId, request.TeamId, token);
        return new AssignTeamResponse();
    }
    
    /// <summary>
    /// Начать игру из лобби
    /// </summary>
    [HttpPost("start-game")]
    public async Task<StartGameFromLobbyResponse> StartGame([FromBody] StartGameFromLobbyRequest request, CancellationToken token)
    {
        var user = _userAuthProvider.GetUser();
        var result = await _lobbyService.StartGame(request.LobbyId, user, token);

        return new StartGameFromLobbyResponse
        {
            Lobby = result.ToDto()
        };
    }
}