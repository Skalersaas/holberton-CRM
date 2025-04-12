using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Interfaces
{
    public interface IModel
    {
        [Required]
        Guid Id { get; set; }
        [Required]
        string Slug { get; set; }
    }
}
