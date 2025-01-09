using HolbertonCRM.Domain.Models;

namespace HolbertonCRM.Models
{
    public class AdmissionNote : EntityBase
    {
        public string Communication { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }
}
