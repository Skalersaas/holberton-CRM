using Domain.Models.Interfaces;
using Domain.Models.JsonTemplates;

namespace Persistance.Data.Interfaces
{
    public interface ISchemaRepository<T> where T : class, ISchema
    {
        Task<T?> CreateAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<(IEnumerable<T> data, int fullCount)> GetAllAsync(SearchModel model);
        Task UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
        void Detach(T entity);
    }
}
