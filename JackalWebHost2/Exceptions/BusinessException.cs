namespace JackalWebHost2.Exceptions;

public abstract class BusinessException : Exception
{
    public abstract string ErrorMessage { get; }
    
    public abstract string ErrorCode { get; }
}