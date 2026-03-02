namespace JackalWebHost2.Exceptions;

public class InvalidTurnNumberException : BusinessException
{
    public override string ErrorMessage => "Неверный номер хода";

    public override string ErrorCode => ErrorCodes.GameNotFound;
}