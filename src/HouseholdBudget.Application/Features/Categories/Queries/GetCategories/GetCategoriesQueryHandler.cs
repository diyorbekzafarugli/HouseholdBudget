using HouseholdBudget.Application.Features.Categories.Common;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Features.Categories.Queries.GetCategories;

public sealed class GetCategoriesQueryHandler(
    ICategoryRepository categoryRepository,
    ILogger<GetCategoriesQueryHandler> logger)
    : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
{
    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request,
        CancellationToken ct)
    {
        logger.LogInformation("Fetching all categories");

        var categories = await categoryRepository.GetAllAsync(ct);

        logger.LogInformation("Fetched {Count} categories", categories.Count());

        return categories.Select(c => new CategoryDto(c.Id, c.Name, c.Type, c.IsDefault));
    }
}