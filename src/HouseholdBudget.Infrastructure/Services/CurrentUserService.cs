using HouseholdBudget.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HouseholdBudget.Infrastructure.Services;

public sealed class CurrentUserService(
    IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid UserId =>
        Guid.Parse(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User is not authenticated."));

    public string Email =>
        httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email)
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

    public string Username =>
        httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)
            ?? throw new UnauthorizedAccessException("User is not authenticated.");
}