using HouseholdBudget.Application.Common.Formatting;

namespace HouseholdBudget.Application.Features.Transactions.Common;

public record CategoryChartItem(string CategoryName, decimal Total)
{
    public string FormattedTotal => AmountFormatter.Format(Total);
    public decimal Percentage { get; init; }
};