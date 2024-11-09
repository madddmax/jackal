namespace JackalWebHost2.Exceptions;

public class TeamIsNotFoundException : BusinessException
{
    public override string ErrorMessage => "Выбранная команда не найдена";

    public override string ErrorCode => ErrorCodes.TeamIsNotFound;
}