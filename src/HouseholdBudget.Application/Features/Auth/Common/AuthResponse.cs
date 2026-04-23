namespace HouseholdBudget.Application.Features.Auth.Common;

public record AuthResponse(
    Guid UserId,
    string Username,
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiry
);