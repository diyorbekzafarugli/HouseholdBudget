using HouseholdBudget.Application.Features.Users.Common;
using MediatR;

namespace HouseholdBudget.Application.Features.Users.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<UserDto>;