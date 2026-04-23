using HouseholdBudget.Domain.Common;
using HouseholdBudget.Domain.Enums;
using HouseholdBudget.Domain.Exceptions;

namespace HouseholdBudget.Domain.Entities;

public sealed class Transaction : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid CategoryId { get; private set; }
    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public string? Comment { get; private set; }
    public DateTime TransactionDate { get; private set; }

    public User User { get; private set; } = null!;
    public Category Category { get; private set; } = null!;

    private Transaction() { }

    public static Transaction Create(
        Guid userId, Guid categoryId, TransactionType type,
        decimal amount, DateTime transactionDate, string? comment = null)
    {
        if (amount <= 0)
            throw new DomainException("Amount must be greater than zero.");

        return new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CategoryId = categoryId,
            Type = type,
            Amount = amount,
            TransactionDate = transactionDate,
            Comment = comment,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(Guid categoryId, decimal amount,
        DateTime transactionDate, string? comment)
    {
        if (amount <= 0)
            throw new DomainException("Amount must be greater than zero.");

        CategoryId = categoryId;
        Amount = amount;
        TransactionDate = transactionDate;
        Comment = comment;
        SetUpdatedAt();
    }
}