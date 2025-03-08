using Domain.Models.Interfaces;
using Domain.Models.JsonTemplates;
using Microsoft.EntityFrameworkCore;

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
<<<<<<< HEAD
        public async Task<T?> GetBySlugAsync(string slug) => await _set.Where(x => x.Slug == slug).SingleAsync();
        public async Task<IEnumerable<T>> GetAllAsync(SearchModel model)
        {
            var set = _set.AsQueryable();
            if (model.Valid())
                set = set.Skip((model.Page - 1) * model.Size).Take(model.Size);
=======
        public async Task<IEnumerable<T>> GetAllAsync() => await _set.ToListAsync();
>>>>>>> parent of 699593b (modify_userController)

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
    }
}
