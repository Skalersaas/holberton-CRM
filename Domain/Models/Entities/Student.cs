using Domain.Models.Interfaces;
using System.Text.Json.Serialization;

namespace Domain.Models.Entities
{
    public class Student : StudentDTO, IModel
    {
        [JsonPropertyName("id")]
        public Guid Guid { get; set; }

        [JsonIgnore]
        public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
    }
    public class StudentDTO 
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

    }
}
