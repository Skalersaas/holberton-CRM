using Domain.Models.Interfaces;
using System.Text.Json.Serialization;

namespace Domain.Models.Entities
{
    public class AdmissionNote : IModel
    {
        [JsonPropertyName("id")]
        public Guid Guid { get; set; }
        public string Slug { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("admissionId")]
        public Guid AdmissionGuid { get; set; }
        public Admission Admission { get; set; } = new();
    }
}
