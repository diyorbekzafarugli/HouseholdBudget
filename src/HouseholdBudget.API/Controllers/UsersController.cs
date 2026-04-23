using HouseholdBudget.Application.Features.Users.Commands.ChangePassword;
using HouseholdBudget.Application.Features.Users.Commands.DeleteUser;
using HouseholdBudget.Application.Features.Users.Commands.UpdateUser;
using HouseholdBudget.Application.Features.Users.Queries.GetCurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdBudget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class UsersController(ISender sender) : BaseController
{
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser(CancellationToken ct)
    {
        var result = await sender.Send(new GetCurrentUserQuery(), ct);
        return OkResponse(result);
    }

    [HttpPut("me")]
    public async Task<IActionResult> Update(
        [FromBody] UpdateUserCommand request, CancellationToken ct)
    {
        await sender.Send(request, ct);
        return NoContent();
    }

    [HttpPut("me/change-password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordCommand request, CancellationToken ct)
    {
        await sender.Send(request, ct);
        return NoContent();
    }

    [HttpDelete("me")]
    public async Task<IActionResult> Delete(CancellationToken ct)
    {
        await sender.Send(new DeleteUserCommand(), ct);
        return NoContent();
    }
}