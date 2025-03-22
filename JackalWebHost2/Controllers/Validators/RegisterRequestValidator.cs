using FluentValidation;
using JackalWebHost2.Controllers.Models;

namespace JackalWebHost2.Controllers.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty()
            .MaximumLength(30);
    }
}