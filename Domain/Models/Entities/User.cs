using Domain.Enums;
using Domain.Models.Interfaces;
using Domain.Validators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Domain.ConstantErrorMessages;

namespace Domain.Models.Entities
{
    public class User : UserDTO, IModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        public string SlugCreating()
        {
            return Name + '-' + Surname;
        }
    }
    public class UserDTO
    {
        public UserRole Role { get; set; }
        [Required(ErrorMessage = FieldRequired)]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public string Surname { get; set; } = string.Empty;
        [PasswordValidation]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public string Login { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}
