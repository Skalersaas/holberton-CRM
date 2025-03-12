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
                404 => ResponseGenerator.NotFound(),
                409 => ResponseGenerator.Conflict(),
                _ => ResponseGenerator.InternalServerError()
            }).Value;

            if (context.Response.Body.Length == 0)
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
