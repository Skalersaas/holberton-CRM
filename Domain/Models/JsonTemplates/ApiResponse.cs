using System.Collections;
using System.Text.Json.Serialization;

namespace Domain.Models.JsonTemplates
{
    public class ApiResponse<T>
    {
        public bool Success { get; init; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Count { get; init; }

        private ApiResponse(bool success, T? data, string? message)
        {
            Success = success;
            Data = data;
            Message = message;
            Count = DataCount(data);
        }

        public static ApiResponse<T> SuccessResponse(T data) =>
            new(true, data, null);

        public static ApiResponse<T> ErrorResponse(string message) =>
            new(false, default, message);

        public static ApiResponse<List<FieldError>> FieldErrorResponse(List<FieldError> errors) =>
            new(false, errors, "Validation failed");

        private static int? DataCount(T? data) =>
            (data is ICollection collection) ? collection.Count : null;

    }

}
