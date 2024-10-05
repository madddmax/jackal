namespace JackalWebHost2.Exceptions;

public class GameNotFoundException : BusinessException
{
    public override string ErrorMessage => "Игра не найдена";

    public override string ErrorCode => ErrorCodes.GameNotFound;
}