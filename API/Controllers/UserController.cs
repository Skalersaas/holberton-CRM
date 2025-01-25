using API.BaseControllers;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;
using static Utilities.Services.ResponseGenerator;
using System.Threading.Tasks;
using Utilities.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IRepository<User> users) : CrudController<User>(users)
    {
        private readonly IRepository<User> _users = users;

        public override async Task<ObjectResult> Update([FromBody] User entity)
        {
            var existingUser = await _users.GetByIdAsync(entity.Guid);

            if (existingUser == null)
                return NotFound(GenerateNotFoundResponse("User", "Guid"));

            _users.Detach(existingUser);

            await _users.UpdateAsync(entity);

            return Ok(GenerateSuccessResponse());
        }

        public override async Task<ObjectResult> New([FromBody] User entity)
        {
            if (!PasswordValidator.ValidatePassword(entity.Password))
                return BadRequest(GenerateErrorResponse("Password does not meet requirements."));

            if (await _users.GetByPredicateAsync(u => u.Username == entity.Username) != null)
                return BadRequest(GenerateErrorResponse("Email already in use."));

            return await base.New(entity);
        }

    }
}
