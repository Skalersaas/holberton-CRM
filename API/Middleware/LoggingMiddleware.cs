using System.Text.Json;

namespace API.Middleware;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Log HTTP Request Information
        context.Request.EnableBuffering();
        var requestInfo = await BuildRequestInfo(context);
        logger.LogInformation(requestInfo);

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

    private async Task<string> BuildRequestInfo(HttpContext context)
    {
        var requestBody = await ReadRequestBodyAsync(context.Request);
        return $"HTTP Request: {context.Request.Method} {context.Request.Path}" +
               (string.IsNullOrWhiteSpace(requestBody) ? "" : $"\nRequest Body: {requestBody}");
    }

    private async Task<string> ReadRequestBodyAsync(HttpRequest request)
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
            var responseLog = $"HTTP Response: {context.Response.StatusCode}" +
                              (string.IsNullOrWhiteSpace(responseText) ? "" : $"\nResponse Body: {FormatJson(responseText)}");

            if (context.Response.StatusCode >= 400)
                logger.LogError(responseLog);
            else
                logger.LogInformation(responseLog);
        }
    }

    private static string FormatJson(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return json;

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            return JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }
        catch
        {
            return json; // Return as is if not valid JSON
        }
    }
}
