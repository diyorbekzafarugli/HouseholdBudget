using HouseholdBudget.Domain.Entities;
using HouseholdBudget.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HouseholdBudget.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(
    AppDbContext context,
    ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        logger.LogDebug("Fetching user by ID: {UserId}", id);
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<User?> GetByEmailAsync(string email,
        CancellationToken ct = default)
    {
        logger.LogDebug("Fetching user by email: {Email}", email);
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        logger.LogDebug("Adding new user: {Username}", user.Username);
        await context.Users.AddAsync(user, ct);
    }

    public Task UpdateAsync(User user, CancellationToken ct = default)
    {
        logger.LogDebug("Updating user: {UserId}", user.Id);
        context.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        logger.LogDebug("Deleting user: {UserId}", id);
        var user = await GetByIdAsync(id, ct);
        if (user is not null)
            context.Users.Remove(user);
    }
}