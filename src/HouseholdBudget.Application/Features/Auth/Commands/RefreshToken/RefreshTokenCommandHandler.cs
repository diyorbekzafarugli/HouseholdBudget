using HouseholdBudget.Application.Features.Auth.Common;
using HouseholdBudget.Application.Interfaces;
using HouseholdBudget.Domain.Exceptions;
using HouseholdBudget.Domain.Interfaces;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Features.Auth.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService,
    IUnitOfWork unitOfWork,
    ILogger<RefreshTokenCommandHandler> logger)
    : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RefreshTokenCommand request,
        CancellationToken ct)
    {
        logger.LogInformation("Refresh token attempt");

        var refreshToken = await refreshTokenRepository.GetByTokenAsync(
            request.Token, ct)
            ?? throw new NotFoundException("RefreshToken", request.Token);

        if (!refreshToken.IsActive)
        {
            logger.LogWarning("Inactive refresh token used for user {UserId}", refreshToken.UserId);
            throw new BusinessRuleException("Refresh token is expired or revoked.");
        }

        var user = await userRepository.GetByIdAsync(refreshToken.UserId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.User), refreshToken.UserId);

        refreshToken.Revoke();

        var newRefreshToken = Domain.Entities.RefreshToken.Create(user.Id);
        await refreshTokenRepository.AddAsync(newRefreshToken, ct);
        await unitOfWork.SaveChangesAsync(ct);

        var (accessToken, expiry) = jwtTokenService.GenerateToken(user);

        logger.LogInformation("Token refreshed for user {UserId}", user.Id);

        return new AuthResponse(user.Id, user.Username, accessToken, newRefreshToken.Token, expiry);
    }
}