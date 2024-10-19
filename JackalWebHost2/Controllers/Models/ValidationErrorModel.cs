using JackalWebHost2.Exceptions;

namespace JackalWebHost2.Controllers.Models;

public class ValidationErrorModel : ErrorModel
{
    public ValidationErrorModel(string errorMessage, ValidationEntryModel[] details) : base(errorMessage, ErrorCodes.ValidationError)
    {
        Details = details;
    }

    public ValidationEntryModel[] Details { get; set; }
}