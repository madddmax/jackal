using FluentValidation;
using JackalWebHost2.Controllers.Models.Map;

namespace JackalWebHost2.Controllers.Validators;

public class CheckLandingRequestValidator : AbstractValidator<CheckLandingRequest>
{
    public CheckLandingRequestValidator()
    {
        RuleFor(x => x.MapSize)
            .NotEmpty()
            .Must(x => x is 5 or 7 or 9 or 11 or 13)
            .WithMessage("MapSize must be 5, 7, 9, 11 or 13");
    }
}