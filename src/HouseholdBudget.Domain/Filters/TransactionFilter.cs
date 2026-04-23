using HouseholdBudget.Domain.Enums;

namespace HouseholdBudget.Domain.Filters;

public record TransactionFilter(
    Guid UserId,
    TransactionType? Type = null,
    IEnumerable<Guid>? CategoryIds = null,
    DateTime? DateFrom = null,
    DateTime? DateTo = null,
    int PageNumber = 1,
    int PageSize = 20
);