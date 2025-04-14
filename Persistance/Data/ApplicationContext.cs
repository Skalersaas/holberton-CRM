using Domain.Models.Entities;
using Domain.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Utilities.Services;

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
                        entityBuilder.Property("Id").ValueGeneratedOnAdd();
                        entityBuilder.HasKey("Id");

                        entityBuilder.HasIndex("Slug").IsUnique();
                    });
                }
            }
            modelBuilder.Entity<User>(user =>
            {
                user.HasIndex("Login").IsUnique();

                user.HasData(new User()
                {
                    Id = Guid.NewGuid(),
                    Login = "Emishkins",
                    Password = PasswordHashGenerator.GenerateHash("Emihskins2!"),
                    Role = Domain.Enums.UserRole.Admin,
                    Name = "Emin",
                    Surname = "Amirov",
                    Slug = "Best-Admin"
                });
            });

            modelBuilder.Entity<Admission>(adm =>
            {
                adm.HasOne(adm => adm.Student)
                    .WithMany()
                    .HasForeignKey(adm => adm.StudentGuid)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                adm.HasOne(adm => adm.User)
                   .WithMany()
                   .HasForeignKey(adm => adm.UserGuid) 
                   .IsRequired();

                adm.HasMany(adm => adm.Notes)
                   .WithOne(note => note.Admission)
                   .HasForeignKey(adm => adm.Id)
                   .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
