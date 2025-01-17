using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace holberton_CRM.Models
{
    [PrimaryKey(nameof(Guid))]
    public class AdmissionChange
    {
        public Guid Guid { get; set; }
        
        public Guid AdmissionGuid {  get; set; }
        public JsonElement Data { get; set; }
    }
}
