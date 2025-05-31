using Domain.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistance.Data.Interfaces;
using System.Linq.Expressions;
using Utilities.DataManipulation;

namespace Persistance.Data.Repositories
{
    public class Repository<T>(ApplicationContext _context) : IRepository<T> 
        where T : class, IModel
    {
        protected readonly DbSet<T> _set = _context.Set<T>();
        public virtual async Task<T?> CreateAsync(T schema)
        {
            try
            {
                schema.Id = Guid.Empty;
                await _set.AddAsync(schema);
                await _context.SaveChangesAsync();
                return schema;
            }
            catch
            {
                return default;
            }
        }
        public async Task<(IEnumerable<T> data, int fullCount)> GetAllAsync(SearchModel model)
        {
            var query = _set.AsQueryable();

            query = QueryMaster<T>.FilterByFields(query, model.Filters);
            query = QueryMaster<T>.OrderByField(query, model.SortedField, model.IsAscending);

            var fullCount = query.Count();
            if (model.PaginationValid())
                query = query.Skip(model.Size * (model.Page - 1)).Take(model.Size);

            return (await query.ToListAsync(), fullCount);
        }
        public int GetCount(SearchModel model)
        {
            var query = _set.AsQueryable();

            query = QueryMaster<T>.FilterByFields(query, model.Filters);
            query = QueryMaster<T>.OrderByField(query, model.SortedField, model.IsAscending);

            var fullCount = query.Count();

            return fullCount;
        }
        public async Task<T?> GetByIdAsync(Guid id) => await _set.FindAsync(id);

        public T? GetByField(string fieldName, object value)
        {
            var property = QueryMaster<T>.GetProperty(fieldName);

            return _set.AsEnumerable().FirstOrDefault(entity => property.GetValue(entity)?.Equals(value) == true);
        }
        public virtual async Task<T> UpdateAsync(T entity)
        {
            _set.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
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

        public void Detach(T entity) => _context.Entry(entity).State = EntityState.Detached;

        public async Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _set;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _set;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }
    }
}
