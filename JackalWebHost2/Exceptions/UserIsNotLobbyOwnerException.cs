namespace JackalWebHost2.Exceptions;

public class UserIsNotLobbyOwnerException : BusinessException
{
    public override string ErrorMessage => "Пользователь должен быть владельцем лобби";

    public override string ErrorCode => ErrorCodes.UserIsNotLobbyOwner;
}