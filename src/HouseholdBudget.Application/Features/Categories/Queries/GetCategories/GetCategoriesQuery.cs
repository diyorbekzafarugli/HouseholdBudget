using MediatR;
using HouseholdBudget.Application.Features.Categories.Common;

namespace HouseholdBudget.Application.Features.Categories.Queries.GetCategories;

public record GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;