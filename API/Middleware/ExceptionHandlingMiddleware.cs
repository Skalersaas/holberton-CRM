using Utilities.Services;
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
            catch
            {
                context.Response.StatusCode = 500;
            }
            finally
            {
                if (context.Response.StatusCode >= 400)
                    await HandleException(context);
            }
        }

        private static async Task HandleException(HttpContext context)
        {
            if (context.Response.HasStarted)
                return;
            var response = (context.Response.StatusCode switch
            {
                400 => ResponseGenerator.BadRequest(),
                401 => ResponseGenerator.Unauthorized(),
                _ => ResponseGenerator.InternalServerError()
            }).Value;


            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
