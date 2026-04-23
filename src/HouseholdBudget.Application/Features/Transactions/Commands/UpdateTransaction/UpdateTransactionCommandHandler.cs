using HouseholdBudget.Application.Interfaces;
using HouseholdBudget.Domain.Exceptions;
using HouseholdBudget.Domain.Interfaces;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Features.Transactions.Commands.UpdateTransaction;

public sealed class UpdateTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    ILogger<UpdateTransactionCommandHandler> logger)
    : IRequestHandler<UpdateTransactionCommand>
{
    public async Task Handle(UpdateTransactionCommand request, CancellationToken ct)
    {
        logger.LogInformation("Updating transaction {TransactionId} for user {UserId}",
            request.Id, currentUserService.UserId);

        var transaction = await transactionRepository.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Transaction), request.Id);

        if (transaction.UserId != currentUserService.UserId)
            throw new NotFoundException(nameof(Domain.Entities.Transaction), request.Id);

        transaction.Update(request.CategoryId, request.Amount, request.TransactionDate, request.Comment);
        await transactionRepository.UpdateAsync(transaction, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Transaction {TransactionId} updated successfully", request.Id);
    }
}