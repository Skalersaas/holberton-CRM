using Application.Services;

namespace API.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) 
            {
                await HandleException(context, ex);
            }
            finally
            {
                if (context.Response.StatusCode >= 400)
                    await HandleException(context);
            }
        }

        private static async Task HandleException(HttpContext context, Exception ex)
        {
            await
#if DEBUG
    context.Response.WriteAsJsonAsync(ResponseGenerator.Error(ex.Message, 500));
#else
    context.Response.WriteAsJsonAsync(ResponseGenerator.Error("Internal server error", 500));
#endif
        }
        private static async Task HandleException(HttpContext context)
        {
            if (context.Response.HasStarted || context.Response.ContentLength > 0) return;

            await context.Response.WriteAsJsonAsync((context.Response.StatusCode switch
            {
                401 => ResponseGenerator.Unauthorized(),
                429 => ResponseGenerator.TooManyRequests(),
                _ => ResponseGenerator.InternalServerError()
            }).Value);
        }
    }
}
