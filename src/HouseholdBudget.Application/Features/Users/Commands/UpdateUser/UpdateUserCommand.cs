using MediatR;

namespace HouseholdBudget.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(string Username, string Email) : IRequest;