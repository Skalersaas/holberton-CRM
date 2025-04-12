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

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? Errors { get; init; }

        private ApiResponse(bool success, T? data, string? message, object? errors)
        {
            Success = success;
            Data = data;
            Message = message;
            Count = DataCount(data);
            Errors = errors;
        }

        public static ApiResponse<T> SuccessResponse(T data) =>
            new(true, data, null, null);

        public static ApiResponse<T> ErrorResponse(string message, object? errors = null) =>
            new(false, default, message, errors);

        private static int? DataCount(T? data) =>
            (data is ICollection collection) ? collection.Count : null;
    }

}
