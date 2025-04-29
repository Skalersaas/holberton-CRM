using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Interfaces
{
    public interface ISchema
    {
        [Required]
        Guid Id { get; set; }
    }
}
