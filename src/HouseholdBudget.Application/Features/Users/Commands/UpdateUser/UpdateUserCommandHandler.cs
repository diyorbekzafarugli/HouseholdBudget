using HouseholdBudget.Application.Interfaces;
using HouseholdBudget.Domain.Exceptions;
using HouseholdBudget.Domain.Interfaces;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    ILogger<UpdateUserCommandHandler> logger)
    : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken ct)
    {
        logger.LogInformation("Updating user with ID: {UserId}", currentUserService.UserId);

        var user = await userRepository.GetByIdAsync(currentUserService.UserId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.User), currentUserService.UserId);

        var existingEmail = await userRepository.GetByEmailAsync(request.Email, ct);
        if (existingEmail is not null && existingEmail.Id != user.Id)
            throw new ConflictException("Email is already taken by another user.");

        user.Update(request.Username, request.Email);

        await userRepository.UpdateAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("User {UserId} updated successfully", user.Id);
    }
}