using System.Text.Json;

namespace holberton_CRM.Models
{
    public class ChangeHistory
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public int AdmissionId { get; set; }
        public Admission? Admission { get; set; }

        public JsonElement Details { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
