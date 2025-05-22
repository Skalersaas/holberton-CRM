using Domain.Models.Interfaces;
using System.Text.Json;
namespace Domain.Models.Entities
{
    public class AdmissionChange : IModel
    {
        public Guid Id { get; set; }

        public Guid AdmissionId { get; set; }
        public DateTime CreatedTime { get; set; }
        public ChangeTemplate[] Data { get; set; }
    }
}
