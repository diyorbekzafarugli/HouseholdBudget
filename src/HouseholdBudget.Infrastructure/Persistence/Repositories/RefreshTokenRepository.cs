using HouseholdBudget.Domain.Entities;
using HouseholdBudget.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenRepository(
    AppDbContext context,
    ILogger<RefreshTokenRepository> logger) : IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByTokenAsync(string token,
        CancellationToken ct = default)
    {
        logger.LogDebug("Fetching refresh token");
        return await context.Set<RefreshToken>()
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token, ct);
    }

    public async Task AddAsync(RefreshToken token, CancellationToken ct = default)
    {
        logger.LogDebug("Adding refresh token for user {UserId}", token.UserId);
        await context.Set<RefreshToken>().AddAsync(token, ct);
    }

    public async Task RevokeAllByUserIdAsync(Guid userId,
        CancellationToken ct = default)
    {
        logger.LogDebug("Revoking all refresh tokens for user {UserId}", userId);
        var tokens = await context.Set<RefreshToken>()
            .Where(r => r.UserId == userId && !r.IsRevoked)
            .ToListAsync<RefreshToken>(ct);

        foreach (var token in tokens)
            token.Revoke();
    }
}