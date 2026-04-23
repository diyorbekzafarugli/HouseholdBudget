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

        var userId = currentUserService.UserId;

        logger.LogDebug("Checking user existence for ID: {UserId}, Request: {Request}",
            userId, requestName);

        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user is null)
        {
            logger.LogWarning("User {UserId} not found in database", userId);
            throw new NotFoundException(nameof(user), userId);
        }

        logger.LogDebug("User {UserId} verified successfully", userId);

        return await next();
    }
}