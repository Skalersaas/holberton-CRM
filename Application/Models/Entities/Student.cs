namespace Domain.Models.Entities
{
    public class Student : IModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
