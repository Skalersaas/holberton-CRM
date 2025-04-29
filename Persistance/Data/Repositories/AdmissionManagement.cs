using Domain.Models.Entities;
namespace Persistance.Data.Repositories
{
    public class AdmissionManagement(
        IRepository<Admission> admissionRepo,
        IRepository<Student> studentRepo,
        IRepository<User> userRepo,
        IRepository<AdmissionChange> admissionChangeRepo,
        IRepository<AdmissionNote> admissionNoteRepo)
    {
        public IRepository<Admission> Admissions { get; } = admissionRepo;
        public IRepository<Student> Students { get; } = studentRepo;
        public IRepository<User> Users { get; } = userRepo;
        public IRepository<AdmissionChange> AdmissionChanges { get; } = admissionChangeRepo;
        public IRepository<AdmissionNote> AdmissionNotes { get; } = admissionNoteRepo;
    }

}
