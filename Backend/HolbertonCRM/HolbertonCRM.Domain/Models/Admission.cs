using HolbertonCRM.Domain.Models;
using HolbertonCRM.Enums;

namespace HolbertonCRM.Models
{
    public class Admission : EntityBase
    {
        //public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Program {  get; set; } = string.Empty;
        public DateOnly ApplyDate { get; set; }
        public string PhoneNumber {  get; set; } = string.Empty;
        public AdmissionStatus Status { get; set; }
        public List<AdmissionNote> Notes { get; set; } = [];
    }
}
