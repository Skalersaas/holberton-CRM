namespace Utilities.Services
{
    public class ResponseGenerator
    {
        public static object GenerateSuccessResponse(object? data = null, string message = "Request successful")
        {
            return new
            {
                message,
                data
            };
        }
        public static object GenerateErrorResponse(string message, int statusCode = 400)
        {
            return new
            {
                message,
                statusCode
            };
        }
        public static object GenerateNotFoundResponse(string entity, string key)
        {
            string message = string.Format("No {0} with such {1}", entity, key);
            return GenerateErrorResponse(message, statusCode: 404);
        }

    }
}