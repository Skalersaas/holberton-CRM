using Domain.Models.Interfaces;
using Domain.Models.JsonTemplates;
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
    }
}
