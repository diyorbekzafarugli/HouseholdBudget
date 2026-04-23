using HouseholdBudget.Application.Interfaces;
using HouseholdBudget.Domain.Exceptions;
using HouseholdBudget.Domain.Interfaces;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    ILogger<DeleteUserCommandHandler> logger)
    : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken ct)
    {
        logger.LogInformation("Deleting user {UserId}", currentUserService.UserId);

        var user = await userRepository.GetByIdAsync(currentUserService.UserId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.User),
            currentUserService.UserId);

        await userRepository.DeleteAsync(user.Id, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("User {UserId} deleted successfully",
            currentUserService.UserId);
    }
}