using holberton_CRM.Data;
using holberton_CRM.Models;
using Microsoft.AspNetCore.Mvc;

namespace holberton_CRM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IRepository<User> _users) : CrudController<User>(_users)
    {
        
    }
}
