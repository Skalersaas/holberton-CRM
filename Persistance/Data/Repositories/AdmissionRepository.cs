using Domain.Models.Entities;
using Domain.Models.JsonTemplates;

namespace Persistance.Data.Repositories
{
    public class AdmissionRepository(ApplicationContext context) : Repository<Admission>(context)
    {
        public override async Task<(IEnumerable<Admission> data, int fullCount)> GetAllAsync(SearchModel model)
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
