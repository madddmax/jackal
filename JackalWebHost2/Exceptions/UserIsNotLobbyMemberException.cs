namespace JackalWebHost2.Exceptions;

public class UserIsNotLobbyMemberException : BusinessException
{
    public override string ErrorMessage => "Пользователь не имеет доступа в комнату";

    public override string ErrorCode => ErrorCodes.UserIsNotLobbyMember;
}