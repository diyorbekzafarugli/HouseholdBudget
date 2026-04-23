using HouseholdBudget.Domain.Enums;
using MediatR;

namespace HouseholdBudget.Application.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(Guid Id, string Name, TransactionType Type)
    : IRequest;