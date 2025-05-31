using Domain.Enums;
using Domain.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    public class Admission : IModel
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;

        [NotMapped]
        public string FirstName { get; set; } = string.Empty;

        [NotMapped]
        public string LastName { get; set; } = string.Empty;
        public string Program { get; set; } = string.Empty;
        public AdmissionStatus Status { get; set; }
        public DateTime ApplyDate { get; set; }

        public Guid? StudentId { get; set; }
        public Student? Student { get; set; }
        
        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public List<AdmissionNote> Notes { get; set; } = [];
        public List<AdmissionChange> Changes { get; set; } = [];

        //public string BuildSlug()
        //{
        //    return (Student!.Slug + '-' + Program).ToLower().Trim();
        //

        public string BuildSlug()
        {
            return (FirstName + '-' + LastName + '-' + Program).ToLower().Trim();
        }

        public void GetFirstAndLastName()
        {
            var parts = Slug.Split('-');

            if (parts.Length >= 3)
            {
                FirstName = parts[0];
                LastName = parts[1];
            }
        }
    }
}
