using API.BaseControllers;
using Domain.Models.Entities;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;
using Utilities.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType<ApiResponse<User>>(StatusCodes.Status200OK)]
    public class UserController(IRepository<User> _users) : 
        CrudController<User, UserDTO>(_users)
    {
        [ProducesResponseType<ApiResponse<IEnumerable<User>>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> GetAll([FromQuery] SearchModel model)
        {
            return await base.GetAll(model);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> New([FromBody] UserDTO entity)
        {
            var user = Mapper.FromDTO<User, UserDTO>(entity);
            user.Password = PasswordHashGenerator.GenerateHash(entity.Password);

            var created = await _users.CreateAsync(user);
            if (created == default)
                return ResponseGenerator.Conflict("User with such slug exists");

            return ResponseGenerator.Ok(entity);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status200OK)]
        public ObjectResult Login([FromForm] string login,[FromForm] string  password)
        {
            var user =  _users.GetByField("login", login);
            if (user == null)
                return ResponseGenerator.NotFound("Entity with such slug was not found");

            if (PasswordHashGenerator.VerifyPassword(user.Password, password))
                return ResponseGenerator.Ok(JwtTokenGenerator.GenerateJwtToken(login));


            return ResponseGenerator.BadRequest("Wrong password");
        }
    }
}
