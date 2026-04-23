using HouseholdBudget.Domain.Entities;

namespace HouseholdBudget.Application.Interfaces;

public interface IJwtTokenService
{
    (string Token, DateTime Expiry) GenerateToken(User user);
}