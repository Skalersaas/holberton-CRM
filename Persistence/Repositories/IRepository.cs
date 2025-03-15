using Application.JsonTemplates;

namespace Persistence.Repositories
{
    public interface IRepository<T>
    {
        Task<T?> CreateAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetBySlugAsync(string id);
        T? GetByField(string fieldName, object value);
        Task<IEnumerable<T>> GetAllAsync(SearchModel model);
        Task UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
        void Detach(T entity);
    }
}
