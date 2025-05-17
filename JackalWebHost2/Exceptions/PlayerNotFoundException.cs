namespace JackalWebHost2.Exceptions;

public class PlayerNotFoundException : BusinessException
{
    public override string ErrorMessage => "Игрок не найден";

    public override string ErrorCode => ErrorCodes.PlayerNotFound;
}