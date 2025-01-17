using holberton_CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace holberton_CRM.Data.Repositories
{
    public class AdmissionRepository(ApplicationContext _context) : IRepository<Admission>
    {
        public async Task<Admission> AddAsync(Admission entity)
        {
            await _context.Admissions.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var admission = await GetByIdAsync(id);
            if (admission != null)
            {
                _context.Admissions.Remove(admission);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Admission>> GetAllAsync()
        {
            return await _context.Admissions.ToListAsync();
        }

        public async Task<Admission?> GetByIdAsync(Guid id)
        {
            return await _context.Admissions.FindAsync(id);
        }

        public async Task UpdateAsync(Admission entity)
        {
            _context.Admissions.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
