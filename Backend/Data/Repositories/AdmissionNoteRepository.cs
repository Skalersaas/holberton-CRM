using holberton_CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace holberton_CRM.Data.Repositories
{
    public class AdmissionNoteRepository(ApplicationContext _context):IRepository<AdmissionNote>
    {
        public async Task<AdmissionNote> AddAsync(AdmissionNote entity)
        {
            await _context.AdmissionNotes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;    
        }

        public async Task DeleteAsync(Guid id)
        {
            var note = await GetByIdAsync(id);

            if (note != null)
            {
                _context.AdmissionNotes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AdmissionNote>> GetAllAsync()
        {
            return await _context.AdmissionNotes.ToListAsync();
        }

        public async Task<AdmissionNote?> GetByIdAsync(Guid id)
        {
            return await _context.AdmissionNotes.FindAsync(id);
        }

        public async Task UpdateAsync(AdmissionNote entity)
        {
            _context.AdmissionNotes.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
