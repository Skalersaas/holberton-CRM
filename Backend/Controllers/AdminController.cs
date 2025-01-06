using holberton_CRM.Data;
using holberton_CRM.Models;
using Microsoft.AspNetCore.Mvc;

namespace holberton_CRM.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController(ApplicationContext context) : ControllerBase
    {
        [HttpGet("users")]
        public ObjectResult GetAllUsers() => Ok(context.GetAllUsers());
        [HttpPost("add-user")]
        public ObjectResult AddUser(User user)
        {
            user.Id = 0;
            user.Guid = Guid.NewGuid();
            context.AddUser(user);
            return Created($"{user.Guid}", user);
        }
        [HttpDelete("{id}/delete")]
        public ObjectResult Delete(Guid id)
        {
            if (!context.RemoveUser(id))
                return NotFound("No such user");

            return Ok("removed user");
        }
        [HttpPatch("users/{id}/edit")]
        public ObjectResult UpdateUser([FromBody] User user,Guid id)
        {
            var usr = context.GetUserById(id);
            if (usr == null)
                return NotFound("User was not found");

            context.UpdateEntity(user, usr, "Id", "Guid");
            return Ok("Updated");
        }
    }
}
