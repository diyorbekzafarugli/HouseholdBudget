using HouseholdBudget.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdBudget.Api.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult OkResponse<T>(T data) =>
        Ok(ApiResponse<T>.Ok(data));

    protected IActionResult CreatedResponse<T>(
        string actionName,object routeValues, T data) =>
        CreatedAtAction(actionName, routeValues, ApiResponse<T>.Ok(data));
}
