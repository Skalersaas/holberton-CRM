using HolbertonCRM.Domain.Models;
using System.Text.Json;

namespace HolbertonCRM.Models
{
    public class ChangeHistory : EntityBase
    {
        //public Guid Guid { get; set; }

        public int AdmissionId { get; set; }
        public Admission? Admission { get; set; }

        public JsonElement Details { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
