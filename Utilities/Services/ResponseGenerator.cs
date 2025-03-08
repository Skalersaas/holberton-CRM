using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Utilities.Services
{
    public class ResponseGenerator
    {
        public static ObjectResult GenerateResponse<T>(bool success, T? data, string? message = null, int statusCode = 200)
        {
            IResponse resp = success ? 
                new DataResponse<T>(data, DataCount(data)) : 
                new ErrorResponse(message);

            return new ObjectResult(resp)
            {
                StatusCode = statusCode
            };
        }
        public static ObjectResult ResponseOK<T>(T? data, string? message = null)
            => GenerateResponse(true, data, message);
        public static ObjectResult ResponseError(string message, int code)
        {
            return GenerateResponse<object>(false, null, message, code);
        }
        public static ObjectResult GenerateBadRequest(string message)
        {
            return ResponseError(message, 400);
        }
        public static ObjectResult GenerateNotFound(string message)
        {
            return ResponseError(message, 404);
        }
        public static ObjectResult GenerateConflict(string message)
        {
            return ResponseError(message, 409);
        }
        public static ObjectResult GenerateInternalServerError(string message)
        {
            return ResponseError(message, 500);
        }
        private static int DataCount<T>(T data)
        {
            return data is ICollection collection ? collection.Count : 1;
        }
    }
    public interface IResponse;
    public record DataResponse<T>(T? Data, int Count): IResponse;
    public record ErrorResponse(string? Message) : IResponse;
}