using Domain.Models.Interfaces;
namespace Domain.Models.Entities
{
    public class AdmissionChange : IModel
    {
        public Guid Id { get; set; }

        public Guid AdmissionId { get; set; }
        public DateTime CreatedTime { get; set; }
        public List<ChangeTemplate> Data { get; set; }
    }
}
