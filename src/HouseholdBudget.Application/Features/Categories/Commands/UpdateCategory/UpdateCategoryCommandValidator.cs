using FluentValidation;
using HouseholdBudget.Application.Resources;
using Microsoft.Extensions.Localization;

namespace HouseholdBudget.Application.Features.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandValidator
    : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localizer["CategoryNameRequired"])
            .MaximumLength(100).WithMessage(localizer["CategoryNameMaxLength"]);

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage(localizer["InvalidEnum"]);
    }
}