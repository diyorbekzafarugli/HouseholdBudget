using HouseholdBudget.Domain.Exceptions;
using HouseholdBudget.Domain.Interfaces;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Features.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteCategoryCommandHandler> logger)
    : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        logger.LogInformation("Deleting category {CategoryId}", request.Id);

        var category = await categoryRepository.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Category), request.Id);

        if (category.IsDefault)
            throw new BusinessRuleException("Default categories cannot be deleted.");

        await categoryRepository.DeleteAsync(request.Id, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Category {CategoryId} deleted successfully", request.Id);
    }
}