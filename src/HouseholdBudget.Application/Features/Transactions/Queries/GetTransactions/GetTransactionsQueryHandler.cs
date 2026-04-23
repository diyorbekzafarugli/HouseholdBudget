using HouseholdBudget.Application.Features.Transactions.Common;
using HouseholdBudget.Application.Interfaces;
using HouseholdBudget.Domain.Enums;
using HouseholdBudget.Domain.Filters;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Features.Transactions.Queries.GetTransactions;

public sealed class GetTransactionsQueryHandler(
    ITransactionRepository transactionRepository,
    ICurrentUserService currentUserService,
    ILogger<GetTransactionsQueryHandler> logger)
    : IRequestHandler<GetTransactionsQuery, TransactionPagedResponse>
{
    public async Task<TransactionPagedResponse> Handle(
        GetTransactionsQuery request, CancellationToken ct)
    {
        logger.LogInformation("Fetching transactions for user {UserId}, Page: {Page}",
            currentUserService.UserId, request.PageNumber);

        var filter = new TransactionFilter(
            currentUserService.UserId,
            request.Type,
            request.CategoryIds,
            request.DateFrom,
            request.DateTo,
            request.PageNumber,
            request.PageSize);

        var (transactions, totalCount) =
            await transactionRepository.GetFilteredAsync(filter, ct);

        var items = transactions.Select(t => new TransactionDto(
            t.Id, t.Type, t.Category.Name,
            t.TransactionDate, t.Amount, t.Comment)).ToList();

        var totalAmount = items.Sum(t => t.Amount);

        var chartData = items
            .GroupBy(t => t.CategoryName)
            .Select(g =>
            {
                var groupTotal = g.Sum(t => t.Amount);
                return new CategoryChartItem(g.Key, groupTotal)
                {
                    Percentage = totalAmount == 0 ? 0
                        : Math.Round(groupTotal / totalAmount * 100, 2)
                };
            })
            .OrderByDescending(c => c.Total)
            .ToList();

        var totalIncome = items
            .Where(t => t.Type == TransactionType.Income)
            .Sum(t => t.Amount);

        var totalExpense = items
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount);

        logger.LogInformation(
            "Fetched {Count} of {Total} transactions. Income: {Income}, Expense: {Expense}",
            items.Count, totalCount, totalIncome, totalExpense);

        return new TransactionPagedResponse
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            ChartData = chartData,
            TotalIncome = totalIncome,
            TotalExpense = totalExpense
        };
    }
}