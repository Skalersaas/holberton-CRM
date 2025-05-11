using Utilities.Responses;
using System.Text.Json;

namespace API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e) 
        {
            Console.WriteLine(e.Message);
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }

        if (!context.Response.HasStarted && context.Response.StatusCode >= 400)
        {
            await WriteErrorResponseAsync(context);
        }
    }

    private static async Task WriteErrorResponseAsync(HttpContext context)
    {
        var response = context.Response.StatusCode switch
        {
            StatusCodes.Status400BadRequest => ResponseGenerator.BadRequest(),
            StatusCodes.Status401Unauthorized => ResponseGenerator.Unauthorized(),
            StatusCodes.Status404NotFound => ResponseGenerator.NotFound(),
            StatusCodes.Status409Conflict => ResponseGenerator.Conflict(),
            _ => ResponseGenerator.InternalServerError()
        };

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(response.Value));
    }
}
