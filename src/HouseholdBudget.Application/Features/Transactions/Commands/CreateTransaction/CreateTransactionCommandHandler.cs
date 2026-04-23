using HouseholdBudget.Application.Interfaces;
using HouseholdBudget.Domain.Entities;
using HouseholdBudget.Domain.Exceptions;
using HouseholdBudget.Domain.Interfaces;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Features.Transactions.Commands.CreateTransaction;

public sealed class CreateTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    ILogger<CreateTransactionCommandHandler> logger)
    : IRequestHandler<CreateTransactionCommand, Guid>
{
    public async Task<Guid> Handle(CreateTransactionCommand request,
        CancellationToken ct)
    {
        logger.LogInformation("Creating transaction for user: {UserId}",
            currentUserService.UserId);

        var category = await categoryRepository.GetByIdAsync(request.CategoryId, ct)
            ?? throw new NotFoundException(nameof(Category), request.CategoryId);

        if (category.Type != request.Type)
            throw new BusinessRuleException("Category type does not match transaction type.");

        var transaction = Transaction.Create(
            currentUserService.UserId,
            request.CategoryId,
            request.Type,
            request.Amount,
            request.TransactionDate,
            request.Comment);

        await transactionRepository.AddAsync(transaction, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Transaction created. ID: {TransactionId}",
            transaction.Id);
        return transaction.Id;
    }
}