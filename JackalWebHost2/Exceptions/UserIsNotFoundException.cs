namespace JackalWebHost2.Exceptions;

public class UserIsNotFoundException : BusinessException
{
    public override string ErrorMessage => "Пользователь не найден";

    public override string ErrorCode => ErrorCodes.UserIsNotFound;
}