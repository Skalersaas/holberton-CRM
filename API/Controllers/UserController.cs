using API.BaseControllers;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IRepository<User> _users) : CrudController<User>(_users)
    {
        
    }
}
