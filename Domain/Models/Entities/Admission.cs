using Domain.Enums;
using Domain.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Domain.ConstantErrorMessages;

namespace Domain.Models.Entities
{
    public class Admission : AdmissionDTO, IModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        
        [JsonIgnore]
        public Student? Student { get; set; }
        
        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public List<AdmissionNote> Notes { get; set; } = [];
        public string BuildSlug()
        {
            return (Student!.Slug + '-' + Program).ToLower().Trim();
        }
    }
    public class AdmissionDTO
    {
        [JsonPropertyName("studentId")]
        public Guid StudentId { get; set; }
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = FieldRequired)]
        public string Program { get; set; } = string.Empty;
        public AdmissionStatus Status { get; set; }
        public DateTime ApplyDate { get; set; }

        public string Slug { get; set; } = string.Empty;
    }
}
