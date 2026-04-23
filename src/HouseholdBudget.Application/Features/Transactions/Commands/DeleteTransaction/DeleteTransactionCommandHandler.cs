using HouseholdBudget.Application.Interfaces;
using HouseholdBudget.Domain.Exceptions;
using HouseholdBudget.Domain.Interfaces;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Features.Transactions.Commands.DeleteTransaction;

public sealed class DeleteTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    ILogger<DeleteTransactionCommandHandler> logger)
    : IRequestHandler<DeleteTransactionCommand>
{
    public async Task Handle(DeleteTransactionCommand request, CancellationToken ct)
    {
        logger.LogInformation("Deleting transaction {TransactionId} for user {UserId}",
            request.Id, currentUserService.UserId);

        var transaction = await transactionRepository.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Transaction), request.Id);

        if (transaction.UserId != currentUserService.UserId)
            throw new NotFoundException(nameof(Domain.Entities.Transaction), request.Id);

        await transactionRepository.DeleteAsync(request.Id, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Transaction {TransactionId} deleted successfully", request.Id);
    }
}