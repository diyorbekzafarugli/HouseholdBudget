using HouseholdBudget.Application.Features.Auth.Common;
using HouseholdBudget.Application.Interfaces;
using HouseholdBudget.Domain.Exceptions;
using HouseholdBudget.Domain.Interfaces;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService,
    IUnitOfWork unitOfWork,
    ILogger<LoginCommandHandler> logger)
    : IRequestHandler<LoginCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        logger.LogInformation("Login attempt for email: {Email}", request.Email);

        var user = await userRepository.GetByEmailAsync(request.Email, ct)
            ?? throw new NotFoundException("User", request.Email);

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            logger.LogWarning("Failed login attempt for email: {Email}", request.Email);
            throw new BusinessRuleException("Invalid email or password.");
        }

        await refreshTokenRepository.RevokeAllByUserIdAsync(user.Id, ct);

        var refreshToken = Domain.Entities.RefreshToken.Create(user.Id);
        await refreshTokenRepository.AddAsync(refreshToken, ct);

        await unitOfWork.SaveChangesAsync(ct);

        var (accessToken, expiry) = jwtTokenService.GenerateToken(user);

        logger.LogInformation("User {UserId} logged in successfully", user.Id);

        return new AuthResponse(user.Id, user.Username, accessToken, refreshToken.Token, expiry);
    }
}