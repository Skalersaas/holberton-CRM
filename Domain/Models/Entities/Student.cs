using Domain.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Domain.ConstantErrorMessages;

namespace Domain.Models.Entities
{
    public class Student : StudentDTO, IModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        public string SlugCreating()
        {
            return Name + '-' + Surname;
        }
    }
    public class StudentDTO 
    {
        [Required(ErrorMessage = FieldRequired)]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public string Surname { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public string Phone { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

    }
}
