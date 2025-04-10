using Domain.Models.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Domain.Models.Entities
{
    public class AdmissionChange : IModel
    {
        [JsonPropertyName("id")]
        public Guid Guid { get; set; }

        [JsonPropertyName("admissionId")]
        public Guid AdmissionGuid { get; set; }
        public string Slug { get; set; } = string.Empty;
        public Admission Admission { get; set; } = new();
        public JsonDocument Data { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }

    public class AdmissionChangeDto
    {
        public DateTime ChangedAt { get; set; }
        public List<ChangeTemplateDto> Changes { get; set; } = new();
    }
    public record ChangeTemplateDto(
        string Field = "",
        string OldValue = "",
        string NewValue = ""
    );
}
