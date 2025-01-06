using holberton_CRM.Models;

namespace holberton_CRM.Data
{
    public partial class ApplicationContext
    {
        public User[] GetAllUsers() => [.. Users];
        public User? GetUserById(Guid id)
        {
            return Users.FirstOrDefault(u => u.Guid == id);
        }
        public void AddUser(User user)
        {
            Users.Add(user);
            SaveChanges();
        }
        public bool RemoveUser(Guid id)
        {
            var usr = GetUserById(id);
            if (usr == null)
                return false;

            Users.Remove(usr);
            SaveChanges();
            return true;
        }
    }
}
