using HouseholdBudget.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HouseholdBudget.Api.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            app.Logger.LogInformation("Applying {Count} pending migrations...",
                pendingMigrations.Count());
            await context.Database.MigrateAsync();
            app.Logger.LogInformation("Migrations applied successfully");
        }
    }
}