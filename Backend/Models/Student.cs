using Microsoft.EntityFrameworkCore;

namespace holberton_CRM.Models
{
    [PrimaryKey(nameof(Guid))]
    public class Student
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Phone {  get; set; } = string.Empty;
    }
}
