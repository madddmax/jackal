namespace JackalWebHost2.Exceptions;

public class LobbyIsClosedException : BusinessException
{
    public override string ErrorMessage => "Комната больше не существует";

    public override string ErrorCode => ErrorCodes.LobbyIsClosed;
}