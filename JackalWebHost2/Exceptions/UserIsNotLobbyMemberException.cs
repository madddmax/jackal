namespace JackalWebHost2.Exceptions;

public class UserIsNotLobbyMemberException : BusinessException
{
    public override string ErrorMessage => "Пользователь не имеет доступа в лобби";

    public override string ErrorCode => ErrorCodes.UserIsNotLobbyMember;
}