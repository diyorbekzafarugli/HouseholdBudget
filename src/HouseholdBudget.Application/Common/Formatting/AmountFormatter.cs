namespace HouseholdBudget.Application.Common.Formatting;

public static class AmountFormatter
{
    public static string Format(decimal amount)
        => amount.ToString("#,##0.00", new System.Globalization.CultureInfo("ru-RU"))
                 .Replace(",", " ");
}