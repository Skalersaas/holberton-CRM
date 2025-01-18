using holberton_CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace holberton_CRM.Data.Repositories
{
    public class Repository<T>(ApplicationContext _context) : IRepository<T> where T : class, IModel
    {
        private readonly DbSet<T> _set = _context.Set<T>();
        public async Task<T> CreateAsync(T entity)
        {
            entity.Guid = Guid.Empty;
            await _set.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        
        public void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public async Task<T?> GetByIdAsync(Guid id) => await _set.FindAsync(id);
        public async Task<IEnumerable<T>> GetAllAsync() => await _set.ToListAsync();

        public async Task UpdateAsync(T entity)
        {
            _set.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(T prev, T next)
        {
            prev = next;
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
