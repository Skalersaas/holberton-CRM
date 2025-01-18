using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace holberton_CRM.Models
{
    [PrimaryKey(nameof(Guid))]
    public class AdmissionChange : IModel
    {
        public Guid Guid { get; set; }
        
        public Guid AdmissionGuid {  get; set; }
        public Admission Admission { get; set; } = new();
        public JsonDocument Data { get; set; }
    }
}
