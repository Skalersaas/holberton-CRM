using System.Text.Json;

namespace API.Middleware;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await next(context);
            return;
        }

        context.Request.EnableBuffering();

        var requestBody = await ReadRequestBodyAsync(context.Request);
        logger.LogInformation("HTTP Request: {Method} {Path}\nRequest Body:\n{Body}",
            context.Request.Method, context.Request.Path, requestBody);

        var originalBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        try
        {
            await next(context);
            await LogResponseAsync(context, responseBodyStream);
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalBodyStream);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
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

    private async Task LogResponseAsync(HttpContext context, MemoryStream responseStream)
    {
        responseStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(responseStream).ReadToEndAsync();

        var level = context.Response.StatusCode >= 400 ? LogLevel.Error : LogLevel.Information;
        var formatted = FormatJson(responseBody);

        logger.Log(level, "HTTP Response: {StatusCode}\nResponse Body:\n{Body}",
            context.Response.StatusCode, formatted);
    }

    private static string FormatJson(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return json;
            var doc = JsonSerializer.Deserialize<JsonElement>(json);
            return JsonSerializer.Serialize(doc, JsonOptions);
        }
        catch
        {
            return json;
        }
    }
}
