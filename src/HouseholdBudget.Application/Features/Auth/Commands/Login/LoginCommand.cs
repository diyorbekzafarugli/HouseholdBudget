using HouseholdBudget.Application.Features.Auth.Common;
using MediatR;

namespace HouseholdBudget.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password)
    : IRequest<AuthResponse>;