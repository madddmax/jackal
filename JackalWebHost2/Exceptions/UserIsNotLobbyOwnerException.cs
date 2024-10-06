namespace JackalWebHost2.Exceptions;

public class UserIsNotLobbyOwnerException : BusinessException
{
    public override string ErrorMessage => "Пользователь должен быть владельцем комнаты";

    public override string ErrorCode => ErrorCodes.LobbyNotFound;
}