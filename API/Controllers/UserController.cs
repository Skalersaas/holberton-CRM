using API.BaseControllers;
using Domain.Models.Entities;
using Domain.Models.JsonTemplates;
using Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;
using Utilities.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType<ApiResponse<User>>(StatusCodes.Status200OK)]
    public class UserController(IRepository<User> _users) : CrudController<User, UserDTO>(_users)
    {
        [ProducesResponseType<ApiResponse<IEnumerable<User>>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> GetAll([FromQuery] SearchModel model)
            => await base.GetAll(model);

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> New([FromBody] UserDTO entity)
        {
            if (!PasswordValidator.IsValid(entity.Password, out var msg))
                return ResponseGenerator.BadRequest(msg);

            var user = Mapper.FromDTO<User, UserDTO>(entity);
            user.Password = PasswordHashGenerator.GenerateHash(entity.Password);

            var created = await _users.CreateAsync(user);
            if (created == default)
                return ResponseGenerator.Conflict("User with such slug exists");

            return ResponseGenerator.Ok("Registered successfully");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
        public ObjectResult Login([FromBody] LoginRequest request)
        {
            var user = _users.GetByField("login", request.Login);
            if (user == null)
                return ResponseGenerator.NotFound("User not found");

            if (!PasswordHashGenerator.VerifyPassword(user.Password, request.Password))
                return ResponseGenerator.BadRequest("Wrong password");

            return ResponseGenerator.Ok(new { accessToken = JwtTokenGenerator.GenerateJwtToken(user.Login) });
        }

        [HttpPut("update")]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> Update([FromBody] User entity)
        {
            var existingUser = await _users.GetByIdAsync(entity.Guid);
            if (existingUser == null)
                return ResponseGenerator.NotFound("User not found");

            entity.Password = existingUser.Password;
            await _users.UpdateAsync(entity);
            return ResponseGenerator.Ok(entity);
        }

        [HttpPost("changepassword")]
        public async Task<ObjectResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var user = _users.GetByField("login", request.Login);
            if (user == null)
                return ResponseGenerator.NotFound("User not found");

            if (!PasswordHashGenerator.VerifyPassword(user.Password, request.OldPassword))
                return ResponseGenerator.BadRequest("Old password is incorrect");

            if (!PasswordValidator.IsValid(request.NewPassword, out var msg))
                return ResponseGenerator.BadRequest(msg);

            user.Password = PasswordHashGenerator.GenerateHash(request.NewPassword);
            await _users.UpdateAsync(user);

            return ResponseGenerator.Ok("Password changed successfully");
        }

        [HttpGet("me")]
        public ObjectResult GetInfoByToken()
        {
            var login = User.Identity?.Name;
            if (string.IsNullOrEmpty(login))
                return ResponseGenerator.Unauthorized();

            var user = _users.GetByField("login", login);
            return user == null
                ? ResponseGenerator.NotFound("User not found")
                : ResponseGenerator.Ok(user);
        }
    }

}