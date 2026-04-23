using HouseholdBudget.Application.Features.Transactions.Common;
using HouseholdBudget.Domain.Enums;
using MediatR;

namespace HouseholdBudget.Application.Features.Transactions.Queries.GetTransactions;

public record GetTransactionsQuery(
    Guid UserId,
    TransactionType? Type,
    IEnumerable<Guid>? CategoryIds,
    DateTime? DateFrom,
    DateTime? DateTo,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<TransactionPagedResponse>;