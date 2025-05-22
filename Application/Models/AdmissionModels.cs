using Domain.Enums;
using Domain.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using static Domain.ConstantErrorMessages;

namespace Application.Models
{
    public class AdmissionCreate
    {
        [Required(ErrorMessage = FieldRequired)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = FieldRequired)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = FieldRequired)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = FieldRequired)]
        public AdmissionProgram Program { get; set; }

        public AdmissionStatus Status { get; set; }

        public string ApplyDate { get; set; } = DateTime.Now.ToString();

        [Required(ErrorMessage = FieldRequired)]
        public Guid UserId { get; set; }
    }
    public class AdmissionUpdate: IModel
    {
        [Required(ErrorMessage = FieldRequired)]
        public Guid Id { get; set; }
        [Required(ErrorMessage = FieldRequired)]
        public string Program { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public Guid StudentId { get; set; }
        [Required(ErrorMessage = FieldRequired)]
        public AdmissionStatus Status { get; set; }
    }
    public class AdmissionResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Program { get; set; } = string.Empty;
        public DateTime ApplyDate { get; set; }
        public AdmissionStatus AdmissionStatus { get; set; }
    }
}
