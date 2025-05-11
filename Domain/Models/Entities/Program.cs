using Domain.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using static Domain.ConstantErrorMessages;

namespace Domain.Models.Entities
{
    public class Program : IModel
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        [Required(ErrorMessage = FieldRequired)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = FieldRequired)]
        public Guid UserId { get; set; }
        public User? CreatedBy { get; set; }

        public string BuildSlug()
        {
            return Name.ToLower();
        }
    }
}