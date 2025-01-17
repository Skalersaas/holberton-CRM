using Microsoft.EntityFrameworkCore;

namespace holberton_CRM.Models
{
    [PrimaryKey(nameof(Guid))]
    public class AdmissionNote
    {
        public Guid Guid { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Content { get; set; } = string.Empty;


        public Guid AdmissionGuid { get; set; }
        public Admission Admission { get; set; } = new();
    }
}
