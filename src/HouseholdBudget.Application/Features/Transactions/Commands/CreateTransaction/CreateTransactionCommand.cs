using HouseholdBudget.Domain.Enums;
using MediatR;

namespace HouseholdBudget.Application.Features.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
    Guid UserId,
    Guid CategoryId,
    TransactionType Type,
    decimal Amount,
    DateTime TransactionDate,
    string? Comment
) : IRequest<Guid>;