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
            base.OnModelCreating(modelBuilder);
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

            modelBuilder.Entity<Admission>(adm =>
            {
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
