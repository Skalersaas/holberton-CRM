using holberton_CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace holberton_CRM.Data
{
    public partial class ApplicationContext: DbContext
    {
        public DbSet<User> Users { get; set; } 
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<AdmissionNote> AdmissionNotes { get; set; }
        public DbSet<ChangeHistory> ChangeHistory { get; set; }
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<AdmissionNote>().Property(n => n.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Admission>(adm =>
            {
                adm.Property(a => a.Id).ValueGeneratedOnAdd();
                adm.HasMany(a => a.Notes) 
                    .WithOne()          
                    .HasForeignKey("AdmissionId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<ChangeHistory>(change =>
            {
                change.Property(c => c.Id).ValueGeneratedOnAdd();
                change.HasOne(c => c.Admission)
                    .WithMany()
                    .HasForeignKey(c => c.AdmissionId);
            });
        }

        public void UpdateEntity<T>(T source, T destination, params string[] excludeProperties)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.CanWrite && !excludeProperties.Contains(p.Name));

            foreach (var property in properties)
            {
                var value = property.GetValue(source);
                property.SetValue(destination, value);
            }
            SaveChanges();
        }
    }
}
