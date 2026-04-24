namespace HouseholdBudget.Application.Common.Formatting;

public static class AmountFormatter
{
    public static string Format(decimal amount)
    {
        var formatted = amount.ToString(
            "N2", new System.Globalization.CultureInfo("ru-RU"));

        return formatted.Replace(",", ".");
    }
}