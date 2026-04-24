using HouseholdBudget.Application.Features.Transactions.Commands.CreateTransaction;
using HouseholdBudget.Application.Features.Transactions.Commands.DeleteTransaction;
using HouseholdBudget.Application.Features.Transactions.Commands.UpdateTransaction;
using HouseholdBudget.Application.Features.Transactions.Queries.GetTransactions;
using HouseholdBudget.Application.Features.Transactions.Queries.GetTransactions.GetTransactionById;
using HouseholdBudget.Application.Interfaces;
using HouseholdBudget.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdBudget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class TransactionsController(
    ISender sender,
    ICurrentUserService currentUserService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] TransactionType? type,
        [FromQuery] Guid[]? categoryIds,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo,
        CancellationToken ct)
    {
        var result = await sender.Send(
            new GetTransactionsQuery(currentUserService.UserId, type, categoryIds, dateFrom, dateTo), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await sender.Send(
            new GetTransactionByIdQuery(id, currentUserService.UserId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTransactionCommand request, CancellationToken ct)
    {
        var id = await sender.Send(request, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTransactionCommand request,
        CancellationToken ct)
    {
        var command = request with { Id = id };
        await sender.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await sender.Send(new DeleteTransactionCommand(id, currentUserService.UserId), ct);
        return NoContent();
    }
}