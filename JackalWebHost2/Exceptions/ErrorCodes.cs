namespace JackalWebHost2.Exceptions;

public static class ErrorCodes
{
    public const string ValidationError = "ValidationError";
    public const string GameNotFound = "GameNotFound";
    public const string LobbyNotFound = "LobbyNotFound";
    public const string LobbyIsFull = "LobbyIsFull";
    public const string UserIsNotLobbyMember = "UserIsNotLobbyMember";
    public const string LobbyIsClosed = "LobbyIsClosed";
    public const string UserIsNotLobbyOwner = "UserIsNotLobbyOwner";
    public const string AllLobbyMembersMustHaveTeam = "AllLobbyMembersMustHaveTeam";
    public const string UserIsAlreadyLoggedIn = "UserIsAlreadyLoggedIn";
    public const string UserIsNotFound = "UserIsNotFound";
    public const string TeamIsNotFound = "TeamIsNotFound";
    public const string PlayerNotFound = "PlayerNotFound";
}