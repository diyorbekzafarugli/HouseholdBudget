using HouseholdBudget.Application.Features.Auth.Commands.Login;
using HouseholdBudget.Application.Features.Auth.Commands.RefreshToken;
using HouseholdBudget.Application.Features.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdBudget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(ISender sender) : BaseController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterCommand request, CancellationToken ct)
    {
        var result = await sender.Send(request, ct);
        return OkResponse(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand request, CancellationToken ct)
    {
        var result = await sender.Send(request, ct);
        return OkResponse(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenCommand request, CancellationToken ct)
    {
        var result = await sender.Send(request, ct);
        return OkResponse(result);
    }
}