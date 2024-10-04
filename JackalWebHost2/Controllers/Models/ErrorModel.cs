using JackalWebHost2.Exceptions;

namespace JackalWebHost2.Controllers.Models;

public class ErrorModel
{
    public ErrorModel(string errorMessage, string errorCode)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public ErrorModel(BusinessException exception)
    {
        ErrorMessage = exception.ErrorMessage;
        ErrorCode = exception.ErrorCode;
    }


    public string ErrorMessage { get; }
    
    public string ErrorCode { get; }

    public bool Error => true;
}