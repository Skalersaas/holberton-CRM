using Domain.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using static Domain.ConstantErrorMessages;

namespace Domain.Models.Entities
{
    public class AdmissionNote : IModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = FieldRequired)]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = FieldRequired)]
        public Guid AdmissionId { get; set; }
        public Admission Admission { get; set; } = new();
        public DateTime CreatedOn { get; set; }

        public AdmissionNote(string content, Guid admId)
        {
            CreatedOn = DateTime.UtcNow;
            AdmissionId = admId;
            Content = content;
        }
        public AdmissionNote()
        {
            CreatedOn = DateTime.UtcNow;
        }
    }
}
