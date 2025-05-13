using Utilities.Responses;
using System.Text.Json;

namespace API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
            return;
        }

        await HandleUnauthorizedOrForbiddenAsync(context);
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception e)
    {
        Console.WriteLine($"[Unhandled Exception] {e}");

        if (context.Response.HasStarted)
            return;

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var response = ResponseGenerator.InternalServerError();
        await context.Response.WriteAsync(JsonSerializer.Serialize(response.Value));
    }

    private static async Task HandleUnauthorizedOrForbiddenAsync(HttpContext context)
    {
        if (context.Response.HasStarted)
            return;

        if (context.Response.StatusCode != StatusCodes.Status401Unauthorized &&
            context.Response.StatusCode != StatusCodes.Status403Forbidden)
            return;

        context.Response.ContentType = "application/json";

        var response = context.Response.StatusCode switch
        {
            StatusCodes.Status401Unauthorized => ResponseGenerator.Unauthorized(),
            StatusCodes.Status403Forbidden => ResponseGenerator.Forbidden(),
            _ => ResponseGenerator.InternalServerError()
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response.Value));
    }
}
