using Domain.Models.Interfaces;
using System.Linq.Expressions;
using Utilities.DataManipulation;

namespace Persistance.Data.Interfaces
{
    public interface IRepository<T> where T : class, IModel
    {
        Task<T?> CreateAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);
        T? GetByField(string fieldName, object value);
        int GetCount(SearchModel model);
        Task<(IEnumerable<T> data, int fullCount)> GetAllAsync(SearchModel model);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
        void Detach(T entity);
        Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
    }
}
