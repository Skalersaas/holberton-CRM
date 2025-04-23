using Domain.Models.JsonTemplates;

namespace Persistance.Data
{
    public interface IRepository<T> where T : class
    {
        Task<T?> CreateAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetBySlugAsync(string id);
        T? GetByField(string fieldName, object value);
        Task<(IEnumerable<T> data, int fullCount)> GetAllAsync(SearchModel model);
        Task UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
        void Detach(T entity);
    }
}
