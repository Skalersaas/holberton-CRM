using Domain.Enums;
using Domain.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models.Entities
{
    public class User : UserDTO, IModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

    }
    public class UserDTO
    {
        public UserRole Role { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Login { get; set; } = string.Empty;
        [Required]
        public string Slug { get; set; } = string.Empty;
    }
}
