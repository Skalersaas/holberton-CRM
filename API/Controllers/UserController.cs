using API.BaseControllers;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;
using static Utilities.Services.ResponseGenerator;
using System.Threading.Tasks;
using Utilities.Services;
using Microsoft.AspNetCore.Identity;
using Utilities.Enums;
using API.Services.IServices;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("new")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Username))
            {
                return BadRequest(GenerateErrorResponse("Username cannot be empty!"));
            }

            if (!PasswordValidator.ValidatePassword(user.Password))
            {
                return BadRequest(GenerateErrorResponse("Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, and one digit."));
            }

            try
            {
                var result = await _userManager.CreateAsync(user, user.Password);

                if (result.Succeeded)
                {
                    await AssignRole(user.Username, UserRole.Staff.ToString());
                    return Ok(GenerateSuccessResponse());
                }

                var errorMessage = "User cannot be created: " + string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(GenerateErrorResponse(errorMessage));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenerateErrorResponse(ex.Message));
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest(GenerateErrorResponse("Username and Password cannot be empty!"));
            }

            var user = await GetUserByNameOrEmail(username);

            bool isValid = await _userManager.CheckPasswordAsync(user, password);

            if (user == null || isValid == false)
            {
                return Unauthorized(GenerateErrorResponse("Invalid username or password!"));
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            return Ok(GenerateSuccessResponse(user, token));
        }


        public async Task<bool> AssignRole(string username, string roleName)
        {
            var user = GetUserByNameOrEmail(username).GetAwaiter().GetResult();
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;

        }
        public async Task<User> GetUserByNameOrEmail(string userNameOrEmail)
        {
            return await _userManager.FindByNameAsync(userNameOrEmail) ?? await _userManager.FindByEmailAsync(userNameOrEmail);
        }

        public List<User> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }
    }
}
