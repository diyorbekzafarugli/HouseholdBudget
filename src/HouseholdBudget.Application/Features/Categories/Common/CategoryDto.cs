using HouseholdBudget.Domain.Enums;

namespace HouseholdBudget.Application.Features.Categories.Common;

public record CategoryDto(
    Guid Id,
    string Name,
    TransactionType Type,
    bool IsDefault
);