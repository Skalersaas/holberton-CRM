using Domain.Enums;
using Domain.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using static Domain.ConstantErrorMessages;

namespace Application.Models
{
    public class UserCreate
    {
        [Required(ErrorMessage = FieldRequired)]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public string Login { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public UserRole Role { get; set; }
    }
    public class UserUpdate : IModel
    {
        [Required(ErrorMessage = FieldRequired)]
        public Guid Id { get; set; }
        [Required(ErrorMessage = FieldRequired)]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public UserRole Role { get; set; }
    }
    public class UserLogin
    {
        [Required(ErrorMessage = FieldRequired)]
        public string Login { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public string Password { get; set; } = string.Empty;
    }
    public class UserResponse : UserUpdate
    {
        public string Slug { get; set; } = string.Empty;
    }
}