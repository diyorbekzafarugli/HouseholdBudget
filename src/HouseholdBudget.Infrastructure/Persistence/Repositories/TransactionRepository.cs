using HouseholdBudget.Domain.Entities;
using HouseholdBudget.Domain.Filters;
using HouseholdBudget.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HouseholdBudget.Infrastructure.Persistence.Repositories;

public sealed class TransactionRepository(AppDbContext context)
    : ITransactionRepository
{
    public async Task<(IEnumerable<Transaction> Items, int TotalCount)> GetFilteredAsync(
        TransactionFilter filter, CancellationToken ct = default)
    {
        var query = context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == filter.UserId)
            .AsQueryable();

        if (filter.Type.HasValue)
            query = query.Where(t => t.Type == filter.Type.Value);

        if (filter.CategoryIds?.Any() == true)
            query = query.Where(t => filter.CategoryIds.Contains(t.CategoryId));

        if (filter.DateFrom.HasValue)
            query = query.Where(t => t.TransactionDate >= filter.DateFrom.Value);

        if (filter.DateTo.HasValue)
            query = query.Where(t => t.TransactionDate <= filter.DateTo.Value);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(t => t.TransactionDate)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync<Transaction>(ct);

        return (items, totalCount);
    }

    public async Task<Transaction?> GetByIdAsync(Guid id, 
        CancellationToken ct = default)
        => await context.Transactions
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task AddAsync(Transaction transaction, CancellationToken ct = default)
        => await context.Transactions.AddAsync(transaction, ct);

    public Task UpdateAsync(Transaction transaction, CancellationToken ct = default)
    {
        context.Transactions.Update(transaction);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var transaction = await GetByIdAsync(id, ct);
        if (transaction is not null)
            context.Transactions.Remove(transaction);
    }
}