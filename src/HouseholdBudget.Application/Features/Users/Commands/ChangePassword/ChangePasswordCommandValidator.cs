using FluentValidation;
using HouseholdBudget.Application.Resources;
using Microsoft.Extensions.Localization;

namespace HouseholdBudget.Application.Features.Users.Commands.ChangePassword;

public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage(localizer["CurrentPasswordRequired"]);

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage(localizer["PasswordRequired"])
            .MinimumLength(8)
            .WithMessage(localizer["PasswordMinLength"])
            .Must(p => !p.Contains(' '))
            .WithMessage(localizer["PasswordNoSpaces"])
            .Must(PasswordValidator.HasUppercase)
            .WithMessage(localizer["PasswordUppercase"])
            .Must(PasswordValidator.HasLowercase)
            .WithMessage(localizer["PasswordLowercase"])
            .Must(PasswordValidator.HasDigit)
            .WithMessage(localizer["PasswordDigit"])
            .Must(PasswordValidator.HasSpecialChar)
            .WithMessage(localizer["PasswordSpecialChar"]);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage(localizer["PasswordRequired"])
            .Equal(x => x.NewPassword)
            .WithMessage(localizer["PasswordsDoNotMatch"]);
    }
}