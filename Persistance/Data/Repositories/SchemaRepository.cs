using Domain.Models.Interfaces;
using Domain.Models.JsonTemplates;
using Microsoft.EntityFrameworkCore;
using Persistance.Data.Interfaces;
using Utilities.Services;

namespace Persistance.Data.Repositories
{
    public class SchemaRepository<T>(ApplicationContext _context) : ISchemaRepository<T> 
        where T : class, ISchema
    {
        protected readonly DbSet<T> _set = _context.Set<T>();
        protected readonly ApplicationContext _context = _context;
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
        public async Task<T?> GetByIdAsync(Guid id) => await _set.FindAsync(id);

        public virtual async Task UpdateAsync(T entity)
        {
            _set.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
