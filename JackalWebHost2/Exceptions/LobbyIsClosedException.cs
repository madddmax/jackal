namespace JackalWebHost2.Exceptions;

public class LobbyIsClosedException : BusinessException
{
    public override string ErrorMessage => "Лобби больше не существует";

    public override string ErrorCode => ErrorCodes.LobbyIsClosed;
}