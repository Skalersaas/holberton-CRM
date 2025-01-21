using System.Text.Json.Serialization;
using Utilities.Enums;
namespace Domain.Models.Entities
{
    public class Admission : IModel
    {
        public Guid Guid { get; set; }

        public Guid StudentGuid { get; set; }
        [JsonIgnore]
        public Student Student { get; set; } = new();
        public Guid UserGuid { get; set; }
        [JsonIgnore]
        public User User { get; set; } = new();

        public string Program { get; set; } = string.Empty;
        public AdmissionStatus Status { get; set; }
        public DateTime ApplyDate { get; set; }
        [JsonIgnore]
        public List<AdmissionNote> Notes { get; set; } = [];
    }
}
