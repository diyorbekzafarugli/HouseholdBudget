using FluentValidation;
using HouseholdBudget.Domain.Exceptions;

namespace HouseholdBudget.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode;
        string message;

        switch (exception)
        {
            case ValidationException ve:
                statusCode = StatusCodes.Status400BadRequest;
                message = string.Join("; ", ve.Errors.Select(e => e.ErrorMessage));
                break;
            case NotFoundException ne:
                statusCode = StatusCodes.Status404NotFound;
                message = ne.Message;
                break;
            case ConflictException ce:
                statusCode = StatusCodes.Status409Conflict;
                message = ce.Message;
                break;
            case BusinessRuleException be:
                statusCode = StatusCodes.Status422UnprocessableEntity;
                message = be.Message;
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                message = "An unexpected error occurred.";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new { error = message });
    }
}