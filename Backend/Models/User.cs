using holberton_CRM.Enums;
using Microsoft.EntityFrameworkCore;

namespace holberton_CRM.Models
{
    [PrimaryKey(nameof(Guid))]
    public class User : IModel
    {
        public Guid Guid { get; set; }
        public UserRole Role { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Login {  get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
