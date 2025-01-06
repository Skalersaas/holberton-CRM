using holberton_CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace holberton_CRM.Data
{
    public partial class ApplicationContext
    {
        public Admission[] GetAllAdmissions()
        {
            return [.. Admissions.Include(a => a.Notes)];
        }
        public Admission? GetAdmissionById(Guid id, bool includeNotes = false)
        {
            var query = Admissions.AsQueryable();
            if (includeNotes)
                query.Include(a => a.Notes);

            return query.FirstOrDefault(a => a.Guid == id);
        }
        public bool AddNoteToAdmission(Guid id, AdmissionNote note)
        {
            var adm = GetAdmissionById(id);
            if (adm == null)
                return false;

            adm.Notes.Add(note);
            SaveChanges();
            return true;
        }
        public void AddAdmission(Admission adm)
        {
            Admissions.Add(adm);
            SaveChanges();
        }
        public bool RemoveAdmission(Guid id)
        {
            var adm = GetAdmissionById(id);
            if (adm == null)
                return false;

            Admissions.Remove(adm);
            SaveChanges();
            return true;
        }
    }
}
