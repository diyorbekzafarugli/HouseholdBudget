using HouseholdBudget.Application.Common.Formatting;
using HouseholdBudget.Domain.Enums;

namespace HouseholdBudget.Application.Features.Transactions.Common;

public record TransactionDto(
    Guid Id,
    TransactionType Type,
    string CategoryName,
    DateTime TransactionDate,
    decimal Amount,
    string? Comment
)
{
    public string FormattedAmount => AmountFormatter.Format(Amount);
    public string FormattedDate => TransactionDate.ToString("dd.MM.yyyy");
};