using holberton_CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace holberton_CRM.Data
{
    public partial class ApplicationContext
    {
        public ChangeHistory[] GetAdmissionHistory(Guid id) => 
            [.. ChangeHistory
                .Include(ch => ch.Admission)
                .Where(ch => ch.Admission != null && ch.Admission.Guid == id)];
    }
}
