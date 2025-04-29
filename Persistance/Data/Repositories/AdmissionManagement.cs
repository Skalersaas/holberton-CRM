using Domain.Models.Entities;
using Persistance.Data.Interfaces;
namespace Persistance.Data.Repositories
{
    public class AdmissionManagement(
        IRepository<Admission> admissionRepo,
        IRepository<Student> studentRepo,
        IRepository<User> userRepo,
        ISchemaRepository<AdmissionChange> admissionChangeRepo,
        ISchemaRepository<AdmissionNote> admissionNoteRepo)
    {
        public IRepository<Admission> Admissions { get; } = admissionRepo;
        public IRepository<Student> Students { get; } = studentRepo;
        public IRepository<User> Users { get; } = userRepo;
        public ISchemaRepository<AdmissionChange> AdmissionChanges { get; } = admissionChangeRepo;
        public ISchemaRepository<AdmissionNote> AdmissionNotes { get; } = admissionNoteRepo;
    }

}
