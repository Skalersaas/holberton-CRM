using Domain.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistance.Data.Interfaces;
using Utilities.Services;
namespace Persistance.Data.Repositories
{
    public class Repository<T>(ApplicationContext context) : SchemaRepository<T>(context), IRepository<T>
        where T : class, IModel
    {
        public override async Task<T?> CreateAsync(T entity)
        {
            entity.Slug = await GetBuildedSlug(entity);
            var baseSlug = entity.BuildSlug();
            
            entity.Slug = baseSlug + '-' + 
                await _set.CountAsync(e => e.Slug.StartsWith(baseSlug));
            return await base.CreateAsync(entity);
        }
        public async Task<T?> GetBySlugAsync(string slug) => await _set.FirstOrDefaultAsync(x => x.Slug == slug);
        public T? GetByField(string fieldName, object value)
        {
            var property = QueryMaster<T>.GetProperty(fieldName);

            return _set.AsEnumerable().FirstOrDefault(entity => property.GetValue(entity)?.Equals(value) == true);
        }
        public override async Task<T> UpdateAsync(T entity)
        {
            var found = await GetByIdAsync(entity.Id) ?? throw new InvalidOperationException("Entity not found");
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name == nameof(entity.Id))
                    continue;

                // Skip navigation or collection properties if needed
                if (!prop.PropertyType.IsValueType && prop.PropertyType != typeof(string))
                    continue;

                var newValue = (prop.Name == nameof(entity.Slug))
                    ? await GetBuildedSlug(entity)
                    : prop.GetValue(entity);
                
                
                prop.SetValue(found, newValue);
            }
            await _context.SaveChangesAsync();
            return found;
        }

        private async Task<string> GetBuildedSlug(T entity)
        {
            var baseSlug = entity.BuildSlug();
            return baseSlug + '-' + await _set.CountAsync(e => e.Slug.StartsWith(baseSlug));
        }
    }
}
