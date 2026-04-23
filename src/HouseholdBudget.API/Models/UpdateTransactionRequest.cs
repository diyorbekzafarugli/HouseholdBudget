namespace HouseholdBudget.Api.Models;

public record UpdateTransactionRequest(
    Guid CategoryId,
    decimal Amount,
    DateTime TransactionDate,
    string? Comment
);