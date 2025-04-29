using Domain.Models.Interfaces;

namespace Domain.Models.Entities
{
    public class Program : IModel
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public Guid UserId { get; set; }
        public User? CreatedBy { get; set; }

        public string BuildSlug()
        {
            return Name;
        }
    }
}