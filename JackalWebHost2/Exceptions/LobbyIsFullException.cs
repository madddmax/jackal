namespace JackalWebHost2.Exceptions;

public class LobbyIsFullException : BusinessException
{
    public override string ErrorMessage => "Комната заполнена";

    public override string ErrorCode => ErrorCodes.LobbyIsFull;
}