using HouseholdBudget.Application.Common.Formatting;
using HouseholdBudget.Application.Common.Models;

namespace HouseholdBudget.Application.Features.Transactions.Common;

public sealed class TransactionPagedResponse : PagedResponse<TransactionDto>
{
    public IReadOnlyList<CategoryChartItem> ChartData { get; init; } = [];
    public decimal TotalIncome { get; init; }
    public decimal TotalExpense { get; init; }
    public decimal Balance => TotalIncome - TotalExpense;
    public string FormattedTotalIncome => AmountFormatter.Format(TotalIncome);
    public string FormattedTotalExpense => AmountFormatter.Format(TotalExpense);
    public string FormattedBalance => AmountFormatter.Format(Balance);
}