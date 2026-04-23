using FluentValidation;
using HouseholdBudget.Application.Resources;
using Microsoft.Extensions.Localization;

namespace HouseholdBudget.Application.Features.Transactions.Commands.CreateTransaction;

public sealed class CreateTransactionCommandValidator
    : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator(
        IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage(localizer["AmountPositive"]);

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage(localizer["FieldRequired"]);

        RuleFor(x => x.TransactionDate)
            .NotEmpty()
            .WithMessage(localizer["DateRequired"]);

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage(localizer["InvalidEnum"]);
    }
}