using System.Text.Json;
namespace Domain.Models.Entities
{
    public class AdmissionChange : IModel
    {
        public Guid Guid { get; set; }

        public Guid AdmissionGuid { get; set; }
        public Admission Admission { get; set; } = new();
        public JsonDocument Data { get; set; }
    }
}
