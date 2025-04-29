using Domain.Models.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Domain.Models.Entities
{
    public class AdmissionChange : ISchema
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("admissionId")]
        public Guid AdmissionId { get; set; }
        public Admission? Admission { get; set; } = null;
        public DateTime CreatedTime { get; set; }
        public JsonDocument Data { get; set; }
    }
}
