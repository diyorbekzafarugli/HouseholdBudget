using HouseholdBudget.Application.Features.Auth.Common;
using MediatR;

namespace HouseholdBudget.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string Token) : IRequest<AuthResponse>;