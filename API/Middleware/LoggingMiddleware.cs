using System.Text.Json;

namespace API.Middleware;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{

    private static readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    public async Task InvokeAsync(HttpContext context)
    {
        // Log HTTP Request Information
        context.Request.EnableBuffering();
        logger.LogInformation("HTTP Request: {method} {path} \nRequest Body: {body}",
            context.Request.Method, context.Request.Path, await ReadRequestBodyAsync(context.Request));

        var originalResponseBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        try
        {
            // Proceed with the next middleware
            await next(context);

            // Log the response
            await LogResponse(context, responseBodyStream);

            // Copy response back to the original stream
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        }
        finally
        {
            // Ensure the original response stream is restored
            context.Response.Body = originalResponseBodyStream;
        }
    }

    private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        if (request.ContentLength == null || request.ContentLength == 0)
            return string.Empty;

        request.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);
        return body;
    }

    private async Task LogResponse(HttpContext context, MemoryStream responseBodyStream)
    {
        responseBodyStream.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(responseBodyStream).ReadToEndAsync();

        if (!context.Request.Path.StartsWithSegments("/swagger"))
        {
            logger.Log(context.Response.StatusCode >= 400 ? LogLevel.Error : LogLevel.Information,
                "HTTP Reponse: {statusCode} \nResponse Body: {body}",
                context.Response.StatusCode, FormatJson(responseText));
        }
    }

    private static string FormatJson(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return json;

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            return JsonSerializer.Serialize(jsonElement, options);
        }
        catch
        {
            return json;
        }
    }
}
