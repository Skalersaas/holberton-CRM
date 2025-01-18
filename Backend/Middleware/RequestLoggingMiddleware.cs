namespace holberton_CRM.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogInformation($"HTTP Request: {context.Request.Method} {context.Request.Path}");

        context.Request.EnableBuffering();
        using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
        {
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            if (!string.IsNullOrWhiteSpace(body))
            {
                logger.LogInformation($"Request Body: {body}");
            }
        }

        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await next(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        if (!context.Request.Path.StartsWithSegments("/swagger"))
        {
            logger.LogInformation($"HTTP Response: {context.Response.StatusCode}");
            logger.LogInformation($"Response Body: {responseText}");
        }
        await responseBody.CopyToAsync(originalBodyStream);
    }
}
