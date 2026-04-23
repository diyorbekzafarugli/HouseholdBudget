using HouseholdBudget.Application.Interfaces;
using HouseholdBudget.Domain.Exceptions;
using HouseholdBudget.Domain.Interfaces;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Features.Users.Commands.ChangePassword;

public sealed class ChangePasswordCommandHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    ILogger<ChangePasswordCommandHandler> logger)
    : IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        logger.LogInformation("Changing password for user {UserId}",
            currentUserService.UserId);

        var user = await userRepository.GetByIdAsync(currentUserService.UserId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.User),
            currentUserService.UserId);

        if (!passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
        {
            logger.LogWarning("Incorrect current password for user {UserId}",
                currentUserService.UserId);
            throw new BusinessRuleException("Current password is incorrect.");
        }

        user.ChangePassword(passwordHasher.Hash(request.NewPassword));
        await userRepository.UpdateAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Password changed successfully for user {UserId}",
            currentUserService.UserId);
    }
}