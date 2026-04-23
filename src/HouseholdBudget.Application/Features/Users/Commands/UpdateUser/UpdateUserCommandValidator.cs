using FluentValidation;
using HouseholdBudget.Application.Resources;
using Microsoft.Extensions.Localization;

namespace HouseholdBudget.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage(localizer["FieldRequired"])
            .MinimumLength(3)
            .WithMessage(localizer["UsernameMinLength"])
            .MaximumLength(50)
            .WithMessage(localizer["UsernameMaxLength"]);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localizer["FieldRequired"])
            .EmailAddress()
            .WithMessage(localizer["EmailInvalid"]);
    }
}