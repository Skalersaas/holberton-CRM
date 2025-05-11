using Domain.Enums;
using Domain.Models.Interfaces;
using Domain.Validators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Domain.ConstantErrorMessages;

namespace Domain.Models.Entities
{
    public class User :  IModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public UserRole Role { get; set; }


        [Required(ErrorMessage = FieldRequired)]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = FieldRequired)]
        public string Login { get; set; } = string.Empty;
        [PasswordValidation]
        public string Password { get; set; } = string.Empty;
        public string BuildSlug()
        {
            return (FirstName + '-' + LastName).ToLower().Trim();
        }
    }
}
