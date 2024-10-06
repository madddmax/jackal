namespace JackalWebHost2.Exceptions;

public class LobbyNotFoundException : BusinessException
{
    public override string ErrorMessage => "Комната не найдена";

    public override string ErrorCode => ErrorCodes.LobbyNotFound;
}