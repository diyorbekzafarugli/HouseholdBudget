using HouseholdBudget.Domain.Enums;
using MediatR;

namespace HouseholdBudget.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Name, TransactionType Type)
    : IRequest<Guid>;