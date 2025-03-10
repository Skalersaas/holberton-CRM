using Microsoft.AspNetCore.Mvc;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Http;

namespace Utilities.Services
{
    public class ResponseGenerator
    {
        public static ObjectResult Success<T>(T? data = default, string? message = null)
        {
            var response = ApiResponse<T?>.SuccessResponse(data);
            return new(response) { StatusCode = StatusCodes.Status200OK };
        }

        public static ObjectResult Error(string message, int statusCode = StatusCodes.Status400BadRequest)
        {
            var response = ApiResponse<object>.ErrorResponse(message);
            return new(response) { StatusCode = statusCode };
        }

        public static ObjectResult Ok<T>(T? data)
            => Success(data);


        public static ObjectResult BadRequest(string message = "Bad request")
            => Error(message, StatusCodes.Status400BadRequest);
        
        public static ObjectResult Unauthorized(string message = "Unauthorized")
            => Error(message, StatusCodes.Status401Unauthorized);

        public static ObjectResult NotFound(string message = "Resource not found")
            => Error(message, StatusCodes.Status404NotFound);

        public static ObjectResult Conflict(string message = "Conflict occurred")
            => Error(message, StatusCodes.Status409Conflict);

        public static ObjectResult InternalServerError(string message = "Internal server error")
            => Error(message, StatusCodes.Status500InternalServerError);
    }
}