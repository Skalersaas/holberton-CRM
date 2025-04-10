using Domain.Models.Entities;
using Domain.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Data
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
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IModel).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType, entityBuilder =>
                    {
                        entityBuilder.Property("Guid").ValueGeneratedOnAdd();
                        entityBuilder.HasKey("Guid");

                        entityBuilder.HasIndex("Slug").IsUnique();
                    });
                }
            }
            modelBuilder.Entity<User>(user =>
            {
                user.HasIndex("Login").IsUnique();
            });
            modelBuilder.Entity<Admission>(adm =>
            {
                adm.HasOne(a => a.Student)
                   .WithMany(s => s.Admissions)
                   .HasForeignKey(a => a.StudentGuid)
                   .OnDelete(DeleteBehavior.Cascade);

                adm.HasOne(a => a.User)
                   .WithMany()
                   .HasForeignKey(a => a.UserGuid)
                   .IsRequired();

                adm.HasMany(a => a.Notes)
                   .WithOne(n => n.Admission)
                   .HasForeignKey(n => n.AdmissionGuid)
                   .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
