using HouseholdBudget.Domain.Enums;

namespace HouseholdBudget.Api.Models;

public record CreateTransactionRequest(
    Guid CategoryId,
    TransactionType Type,
    decimal Amount,
    DateTime? TransactionDate,
    string? Comment
);