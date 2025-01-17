using holberton_CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace holberton_CRM.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<AdmissionChange> AdmissionChanges { get; set; }
        public DbSet<AdmissionNote> AdmissionNotes { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().Property(u => u.Guid).ValueGeneratedOnAdd();
            modelBuilder.Entity<Student>().Property(s => s.Guid).ValueGeneratedOnAdd();

            modelBuilder.Entity<AdmissionNote>().Property(note => note.Guid).ValueGeneratedOnAdd();
            modelBuilder.Entity<AdmissionChange>().Property(change => change.Guid).ValueGeneratedOnAdd();

            modelBuilder.Entity<Admission>(adm =>
            {
                adm.Property(adm => adm.Guid).ValueGeneratedOnAdd();
                
                adm.HasOne(adm => adm.Student)
                .WithMany()
                .HasForeignKey(adm => adm.StudentGuid)
                .IsRequired();

                adm.HasOne(adm => adm.User)
                .WithMany()
                .HasForeignKey(adm => adm.UserGuid)
                .IsRequired();

                adm.HasMany(adm => adm.Notes)
                .WithOne(note => note.Admission)
                .HasForeignKey(adm => adm.Guid)
                .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
