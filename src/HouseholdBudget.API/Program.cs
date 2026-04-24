using HouseholdBudget.Api.Extensions;
using HouseholdBudget.Api.Middleware;
using HouseholdBudget.Application.DependencyInjection;
using HouseholdBudget.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddControllers()
    .Services
    .AddJwtAuth(builder.Configuration)
    .AddApiRateLimiting()
    .AddApiLocalization()
    .AddApiHealthChecks(builder.Configuration)
    .AddSwagger();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRequestLocalization();
app.UseRateLimiter();
app.UseSwaggerWithUi();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

await app.ApplyMigrationsAsync();

app.Run();