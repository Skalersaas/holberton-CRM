using Domain.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using static Domain.ConstantErrorMessages;

namespace Application.Models
{
    public class StudentCreate
    {
        [Required(ErrorMessage = FieldRequired)]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public string Phone { get; set; } = string.Empty;
    }
    public class StudentUpdate : StudentCreate, IModel
    {
        public Guid Id { get; set; }
    }
    public class StudentResponse : StudentUpdate
    {
        public string Slug { get; set; } = string.Empty;
        public bool IsEnrolled { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}