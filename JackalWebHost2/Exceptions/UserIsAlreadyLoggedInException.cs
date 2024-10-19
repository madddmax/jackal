namespace JackalWebHost2.Exceptions;

public class UserIsAlreadyLoggedInException : BusinessException
{
    public override string ErrorMessage => "Пользователь уже вошел";

    public override string ErrorCode => ErrorCodes.UserIsAlreadyLoggedIn;
}