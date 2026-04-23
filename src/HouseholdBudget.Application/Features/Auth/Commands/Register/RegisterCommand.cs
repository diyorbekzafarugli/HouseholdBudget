using HouseholdBudget.Application.Features.Auth.Common;
using MediatR;

namespace HouseholdBudget.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Username,
    string Email,
    string Password
) : IRequest<AuthResponse>;