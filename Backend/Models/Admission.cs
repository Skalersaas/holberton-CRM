using holberton_CRM.Enums;
using Microsoft.EntityFrameworkCore;

namespace holberton_CRM.Models
{
    [PrimaryKey(nameof(Guid))]
    public class Admission
    {
        public Guid Guid { get; set; }
        
        public Guid StudentGuid { get; set; }
        public Student Student { get; set; } = new();
        
        public Guid UserGuid { get; set; }
        public User User { get; set; } = new();

        public string Program { get; set; } = string.Empty;
        public AdmissionStatus Status { get; set; }
        public DateTime ApplyDate { get; set; }
        public List<AdmissionNote> Notes { get; set; } = [];
    }
}
