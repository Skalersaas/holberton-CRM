using holberton_CRM.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace holberton_CRM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(UserRepository _context) : ControllerBase
    {
        
    }
}
