using HouseholdBudget.Application.Features.Auth.Common;
using HouseholdBudget.Application.Interfaces;
using HouseholdBudget.Domain.Entities;
using HouseholdBudget.Domain.Exceptions;
using HouseholdBudget.Domain.Interfaces;
using HouseholdBudget.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService,
    IUnitOfWork unitOfWork,
    ILogger<RegisterCommandHandler> logger)
    : IRequestHandler<RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken ct)
    {
        logger.LogInformation("Registering new user with email: {Email}", request.Email);

        var existing = await userRepository.GetByEmailAsync(request.Email, ct);
        if (existing is not null)
            throw new ConflictException("User with this email already exists.");

        var passwordHash = passwordHasher.Hash(request.Password);
        var user = User.Create(request.Username, request.Email, passwordHash);

        await userRepository.AddAsync(user, ct);

        var refreshToken = Domain.Entities.RefreshToken.Create(user.Id);
        await refreshTokenRepository.AddAsync(refreshToken, ct);

        await unitOfWork.SaveChangesAsync(ct);

        var (accessToken, expiry) = jwtTokenService.GenerateToken(user);

        logger.LogInformation("User registered successfully. ID: {UserId}", user.Id);

        return new AuthResponse(user.Id, user.Username, accessToken, refreshToken.Token, expiry);
    }
}