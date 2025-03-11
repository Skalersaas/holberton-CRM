using Domain.Models.Interfaces;
using Domain.Models.JsonTemplates;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Persistance.Data.Repositories
{
    public class Repository<T>(ApplicationContext _context) : IRepository<T> where T : class, IModel
    {
        private readonly DbSet<T> _set = _context.Set<T>();
        public async Task<T?> CreateAsync(T entity)
        {
            try
            {
                entity.Guid = Guid.Empty;
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
            var property = typeof(T).GetProperty(fieldName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                ?? throw new ArgumentNullException(fieldName);

            return _set.AsEnumerable().FirstOrDefault(entity => property.GetValue(entity)?.Equals(value) == true);
        }
        public async Task<IEnumerable<T>> GetAllAsync(SearchModel model)
        {
            var set = string.IsNullOrEmpty(model.SortedField)
                ? _set.AsQueryable()
                : OrderByField(_set, model.SortedField, model.IsAscending);

            if (model.Valid())
                set = set.Skip((model.Page - 1) * model.Size).Take(model.Size);

            return await set.ToListAsync();
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
        private static IQueryable<T> OrderByField(IQueryable<T> source, string fieldName, bool ascending)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, fieldName);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            var result = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type)
                .Invoke(null, [source, lambda]);

            return (IQueryable<T>)result!;
        }
    }
}
