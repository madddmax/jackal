namespace JackalWebHost2.Exceptions;

public class LobbyNotFoundException : BusinessException
{
    public override string ErrorMessage => "Лобби не найдено";

    public override string ErrorCode => ErrorCodes.LobbyNotFound;
}