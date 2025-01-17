namespace holberton_CRM.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Логирование входящего запроса
        logger.LogInformation($"HTTP Request: {context.Request.Method} {context.Request.Path}");

        // Копируем тело запроса, чтобы его можно было прочитать
        context.Request.EnableBuffering();
        using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
        {
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0; // Возвращаем указатель в начало для следующей обработки
            if (!string.IsNullOrWhiteSpace(body))
            {
                logger.LogInformation($"Request Body: {body}");
            }
        }

        // Перехват ответа
        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await next(context); // Передаем управление следующему middleware

        // Логирование ответа
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        logger.LogInformation($"HTTP Response: {context.Response.StatusCode}");
        logger.LogInformation($"Response Body: {responseText}");

        await responseBody.CopyToAsync(originalBodyStream);
    }
}
