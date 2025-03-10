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
                if (context.Response.StatusCode >= 400)
                    await HandleException(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private static async Task HandleException(HttpContext context, Exception? ex = null)
        {
            context.Response.ContentType = "application/json";

            Console.WriteLine($"Error occurred: {ex?.Message ?? context.Response.StatusCode.ToString()}");

            var response = (context.Response.StatusCode switch
            {
                400 => ResponseGenerator.BadRequest(),
                401 => ResponseGenerator.Unauthorized(),
                404 => ResponseGenerator.NotFound(),
                409 => ResponseGenerator.Conflict(),
                _ => ResponseGenerator.InternalServerError()
            }).Value;

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
