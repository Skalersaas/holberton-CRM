using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DB
{
    public class ApplicationContext : DbContext
    {
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
        }
    }
}
