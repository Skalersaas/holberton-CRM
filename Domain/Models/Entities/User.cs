using Domain.Models.Interfaces;
using System.Text.Json.Serialization;
using Utilities.Enums;

namespace Domain.Models.Entities
{
    public class User : UserDTO, IModel
    {
        [JsonPropertyName("id")]
        public Guid Guid { get; set; }

    }
    public class UserDTO
    {
        public UserRole Role { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}
