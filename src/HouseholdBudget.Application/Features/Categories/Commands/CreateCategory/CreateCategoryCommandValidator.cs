using FluentValidation;
using HouseholdBudget.Application.Features.Categories.Commands.CreateCategory;
using HouseholdBudget.Application.Resources;
using Microsoft.Extensions.Localization;

namespace HouseholdBudget.Application.Features.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localizer["CategoryNameRequired"])
            .MaximumLength(100).WithMessage(localizer["CategoryNameMaxLength"]);

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage(localizer["InvalidEnum"]);
    }
}