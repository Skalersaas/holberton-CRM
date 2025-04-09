using Domain.Models.Entities;
using Domain.Models.JsonTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Data.Repositories
{
    public class AdmissionRepository : Repository<Admission>
    {
        public AdmissionRepository(ApplicationContext context) : base(context) { }

        public override async Task<IEnumerable<Admission>> GetAllAsync(SearchModel model)
        {
            if (string.IsNullOrWhiteSpace(model.SortedField))
            {
                model.SortedField = nameof(Admission.ApplyDate);
                model.IsAscending = true;
            }

            return await base.GetAllAsync(model);
        }
    }
}
