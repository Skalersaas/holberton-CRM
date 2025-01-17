using holberton_CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace holberton_CRM.Data.Repositories
{
    public class AdmissionChangeRepository(ApplicationContext _context) : IRepository<AdmissionChange>
    {
        public async Task<AdmissionChange> AddAsync(AdmissionChange entity)
        {
            await _context.AdmissionChanges.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var stud = await GetByIdAsync(id);

            if (stud != null)
            {
                _context.AdmissionChanges.Remove(stud);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AdmissionChange>> GetAllAsync()
        {
            return await _context.AdmissionChanges.ToListAsync();
        }

        public async Task<AdmissionChange?> GetByIdAsync(Guid id)
        {
            return await _context.AdmissionChanges.FindAsync(id);
        }

        public async Task UpdateAsync(AdmissionChange entity)
        {
            _context.AdmissionChanges.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
