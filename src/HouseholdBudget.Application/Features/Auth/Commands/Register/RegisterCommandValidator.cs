using FluentValidation;
using HouseholdBudget.Application.Features.Users.Commands.ChangePassword;
using HouseholdBudget.Application.Resources;
using Microsoft.Extensions.Localization;

namespace HouseholdBudget.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator(IStringLocalizer<SharedResource> localizer)
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

        RuleFor(x => x.Password)
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
    }
}