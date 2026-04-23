using HouseholdBudget.Application.Features.Transactions.Common;
using HouseholdBudget.Application.Interfaces;
using HouseholdBudget.Domain.Exceptions;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Features.Transactions.Queries.GetTransactions.GetTransactionById;

public sealed class GetTransactionByIdQueryHandler(
    ITransactionRepository transactionRepository,
    ICurrentUserService currentUserService,
    ILogger<GetTransactionByIdQueryHandler> logger)
    : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
{
    public async Task<TransactionDto> Handle(GetTransactionByIdQuery request,
        CancellationToken ct)
    {
        logger.LogInformation("Fetching transaction {TransactionId} for user {UserId}",
            request.Id, currentUserService.UserId);

        var t = await transactionRepository.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Transaction), request.Id);

        if (t.UserId != currentUserService.UserId)
            throw new NotFoundException(nameof(Domain.Entities.Transaction), request.Id);

        logger.LogInformation("Transaction {TransactionId} fetched successfully", request.Id);

        return new TransactionDto(t.Id, t.Type, t.Category.Name,
            t.TransactionDate, t.Amount, t.Comment);
    }
}