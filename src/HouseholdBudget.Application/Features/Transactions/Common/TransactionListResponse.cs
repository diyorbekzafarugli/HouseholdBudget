using HouseholdBudget.Application.Common.Formatting;

namespace HouseholdBudget.Application.Features.Transactions.Common;

public record TransactionListResponse(
    IReadOnlyList<TransactionDto> Items,
    IReadOnlyList<CategoryChartItem> ChartData
)
{
    public decimal TotalIncome => Items
        .Where(t => t.Type == Domain.Enums.TransactionType.Income)
        .Sum(t => t.Amount);

    public decimal TotalExpense => Items
        .Where(t => t.Type == Domain.Enums.TransactionType.Expense)
        .Sum(t => t.Amount);

    public decimal Balance => TotalIncome - TotalExpense;

    public string FormattedTotalIncome => AmountFormatter.Format(TotalIncome);
    public string FormattedTotalExpense => AmountFormatter.Format(TotalExpense);
    public string FormattedBalance => AmountFormatter.Format(Balance);
};