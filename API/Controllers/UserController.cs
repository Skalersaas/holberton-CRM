using API.BaseControllers;
using Domain.Models.Entities;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;
<<<<<<< HEAD
using Utilities.Services;
using static Utilities.Services.ResponseGenerator;
=======
>>>>>>> parent of 699593b (modify_userController)

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
<<<<<<< HEAD
    [ProducesResponseType<DataResponse<User>>(StatusCodes.Status200OK)]
    public class UserController(IRepository<User> _users) : 
        CrudController<User, UserDTO>(_users)
    {
        [ProducesResponseType<DataResponse<IEnumerable<User>>>(StatusCodes.Status200OK)]
        public override Task<ObjectResult> GetAll([FromQuery] SearchModel model)
        {
            return base.GetAll(model);
        }
=======
    public class UserController(IRepository<User> _users) : CrudController<User>(_users)
    {
        
>>>>>>> parent of 699593b (modify_userController)
    }
}
