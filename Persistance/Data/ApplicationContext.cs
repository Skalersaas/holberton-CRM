using Domain.Models.Entities;
using Domain.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Utilities.Security;

namespace Persistance.Data
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<AdmissionNote> AdmissionNotes { get; set; }
        public DbSet<AdmissionChange> AdmissionChanges { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IModel).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType, entityBuilder =>
                    {
                        entityBuilder.Property("Id").ValueGeneratedOnAdd();
                        entityBuilder.HasKey("Id");
                    });
                }
            }
            modelBuilder.Entity<User>(user =>
            {
                user.HasIndex("Login").IsUnique();
                user.HasIndex("Slug").IsUnique();

                user.HasData(new User()
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    FirstName = "Emin",
                    LastName = "Amirov",
                    Login = "Emishkins",
                    Slug = "Best-Admin-1",
                    Password = "CNb+bAGhE8gnpSsg4Tj0LCDfSVXmTn8K6lJ6FkUNueugQXpRTwNZTo+QhH0KOK8Z",
                    Role = Domain.Enums.UserRole.Admin
                });
            });

            modelBuilder.Entity<Admission>(adm =>
            {
                adm.HasIndex("Slug").IsUnique();
                adm.HasOne(adm => adm.Student)
                    .WithMany()
                    .HasForeignKey(adm => adm.StudentId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                adm.HasOne(adm => adm.User)
                   .WithMany()
                   .HasForeignKey(adm => adm.UserId)
                   .IsRequired();

                adm.HasMany(adm => adm.Notes)
                   .WithOne(note => note.Admission)
                   .HasForeignKey(adm => adm.Id)
                   .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Student>(student =>
            {
                student.HasIndex("Slug").IsUnique();
            });

        }
    }
}
