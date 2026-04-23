using HouseholdBudget.Application.Features.Users.Common;
using HouseholdBudget.Application.Interfaces;
using HouseholdBudget.Domain.Exceptions;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Features.Users.Queries.GetCurrentUser;

public sealed class GetCurrentUserQueryHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    ILogger<GetCurrentUserQueryHandler> logger)
    : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken ct)
    {
        logger.LogInformation("Fetching current user with ID: {UserId}", currentUserService.UserId);

        var user = await userRepository.GetByIdAsync(currentUserService.UserId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.User), currentUserService.UserId);

        logger.LogInformation("Successfully fetched user: {Username}", user.Username);

        return new UserDto(user.Id, user.Username, user.Email, user.CreatedAt);
    }
}