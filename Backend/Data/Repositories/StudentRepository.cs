using holberton_CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace holberton_CRM.Data.Repositories
{
    public class StudentRepository(ApplicationContext _context) : IRepository<Student>
    {
        public async Task<Student> AddAsync(Student entity)
        {
            entity.Guid = Guid.Empty;
            await _context.Students.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var stud = await GetByIdAsync(id);

            if (stud != null)
            {
                _context.Students.Remove(stud);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task UpdateAsync(Student entity)
        {
            _context.Students.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
