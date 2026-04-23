using HouseholdBudget.Application.Features.Transactions.Common;
using MediatR;

namespace HouseholdBudget.Application.Features.Transactions.Queries.GetTransactions.GetTransactionById;

public record GetTransactionByIdQuery(Guid Id, Guid UserId)
    : IRequest<TransactionDto>;