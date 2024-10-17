namespace JackalWebHost2.Exceptions;

public class LobbyIsFullException : BusinessException
{
    public override string ErrorMessage => "Лобби заполнено";

    public override string ErrorCode => ErrorCodes.LobbyIsFull;
}