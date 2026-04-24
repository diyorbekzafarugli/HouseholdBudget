using HouseholdBudget.Application.Interfaces;
using HouseholdBudget.Domain.Exceptions;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Common.Behaviors;

public sealed class UserExistsBehavior<TRequest, TResponse>(
    ICurrentUserService currentUserService,
    IUserRepository userRepository,
    ILogger<UserExistsBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        if (requestName is "RegisterCommand" or "LoginCommand")
            return await next();

        Guid userId;
        try
        {
            userId = currentUserService.UserId;
        }
        catch (UnauthorizedAccessException)
        {
            return await next();
        }

        logger.LogDebug("Checking user existence for ID: {UserId}", userId);

        var user = await userRepository.GetByIdAsync(userId, CancellationToken.None);
        if (user is null)
        {
            logger.LogWarning("User {UserId} not found", userId);
            throw new NotFoundException(nameof(user), userId);
        }

        return await next();
    }
}