using Domain.Models.Entities;
using Domain.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Data
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<AdmissionChange> AdmissionChanges { get; set; }
        public DbSet<AdmissionNote> AdmissionNotes { get; set; }
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
            });

            modelBuilder.Entity<Admission>(adm =>
            {
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

        }
    }
}
