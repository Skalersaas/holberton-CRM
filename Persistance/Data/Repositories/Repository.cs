using Domain.Models.Interfaces;
using Domain.Models.JsonTemplates;
using Microsoft.EntityFrameworkCore;
using Utilities.Services;
namespace Persistance.Data.Repositories
{
    public class Repository<T>(ApplicationContext _context) : IRepository<T> where T : class, IModel
    {
        private readonly DbSet<T> _set = _context.Set<T>();

        public async Task<T?> CreateAsync(T entity)
        {
            try
            {
                entity.Id = Guid.Empty;
                await _set.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch
            {
                return default;
            }
        }
        public void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
        public async Task<T?> GetByIdAsync(Guid id) => await _set.FindAsync(id);
        public async Task<T?> GetBySlugAsync(string slug) => await _set.FirstOrDefaultAsync(x => x.Slug == slug);
        public T? GetByField(string fieldName, object value)
        {
            var property = QueryMaster<T>.GetProperty(fieldName);

            return _set.AsEnumerable().FirstOrDefault(entity => property.GetValue(entity)?.Equals(value) == true);
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync(SearchModel model)
        {
            var query = _set.AsQueryable();

            query = QueryMaster<T>.FilterByFields(query, model.Filters);
            query = QueryMaster<T>.OrderByField(query, model.SortedField, model.IsAscending);

            if (model.PaginationValid())
                query = query.Skip(model.Size * (model.Page - 1)).Take(model.Size);

            return await query.ToListAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            _set.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
                return false;

            _set.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
