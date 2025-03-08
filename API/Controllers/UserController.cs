using API.BaseControllers;
using Domain.Models.Entities;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;
using Utilities.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType<DataResponse<User>>(StatusCodes.Status200OK)]
    public class UserController(IRepository<User> _users) : 
        CrudController<User, UserDTO>(_users)
    {
        [ProducesResponseType<DataResponse<IEnumerable<User>>>(StatusCodes.Status200OK)]
        public override Task<ObjectResult> GetAll([FromQuery] SearchModel model)
        {
            return base.GetAll(model);
        }
    }
}
