using MediatR;

namespace HouseholdBudget.Application.Features.Transactions.Commands.UpdateTransaction;

public record UpdateTransactionCommand(
    Guid Id,
    Guid UserId,
    Guid CategoryId,
    decimal Amount,
    DateTime TransactionDate,
    string? Comment
) : IRequest;
