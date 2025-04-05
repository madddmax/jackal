using JackalWebHost2.Controllers.Models.Lobby;
using JackalWebHost2.Models.Lobby;

namespace JackalWebHost2.Controllers.Mappings;

public static class LobbyControllerMappings
{
    public static LobbyModel ToDto(this Lobby lobby) =>
        new()
        {
            Id = lobby.Id,
            OwnerId = lobby.OwnerId,
            LobbyMembers = lobby.LobbyMembers.Values.ToDictionary(x => x.UserId, x => new LobbyMemberModel
            {
                UserId = x.UserId,
                UserName = x.UserName,
                TeamId = x.TeamId
            }),
            GameSettings = lobby.GameSettings,
            NumberOfPlayers = lobby.NumberOfPlayers,
            GameId = lobby.GameId
        };
}