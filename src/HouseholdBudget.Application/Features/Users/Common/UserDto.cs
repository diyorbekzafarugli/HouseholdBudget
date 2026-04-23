namespace HouseholdBudget.Application.Features.Users.Common;

public record UserDto(
    Guid Id,
    string Username,
    string Email,
    DateTime CreatedAt
);