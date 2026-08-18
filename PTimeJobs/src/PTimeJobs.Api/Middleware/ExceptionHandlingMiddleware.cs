using System.Net;
using System.Text.Json;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Middleware;

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
        catch (InvalidOperationException exception)
        {
            // Domain entities (e.g. Job.Create, JobApplication.ChangeStatus) throw
            // InvalidOperationException for broken business rules / bad input — treat
            // that as a client error rather than an unexpected server failure.
            logger.LogWarning(exception, "Business rule violation.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var response = ApiResponse<object>.Failure(exception.Message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception occurred.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = ApiResponse<object>.Failure("An unexpected error occurred.");
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
