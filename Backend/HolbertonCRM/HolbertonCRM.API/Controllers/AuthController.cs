using HolbertonCRM.Application.DTOs.Auth;
using HolbertonCRM.Application.Interfaces;
using HolbertonCRM.Utilities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HolbertonCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _accountService;

        public AuthController(IAuthService accountService)
        {
            _accountService = accountService;
        }

        // Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registrationResult = await _accountService.Register(registerDto);

            if (registrationResult == UserRegistrationResult.Success)
            {
                return StatusCode(201, "Registration successful.");
            }

            return BadRequest("Registration failed.");
        }

        // User login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid login attempt.");
            }

            var loginResult = await _accountService.Login(loginDto);

            if (loginResult.User == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new { loginResult.User, loginResult.Token });
        }

        // Change the user's password
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid change password request.");
            }

            var result = await _accountService.ChangePassword(changePasswordDto);

            if (result.Succeeded)
            {
                return Ok("Password changed successfully.");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.ToString());
            }

            return BadRequest(ModelState);
        }

        // Initiate password reset by sending an email with a reset link
        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordDto forgetPasswordDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid request.");
        //    }

        //    var user = await _accountService.GetUserByNameOrEmail(forgetPasswordDto.Email);

        //    if (user == null)
        //    {
        //        return NotFound("User not found.");
        //    }

        //    var token = await _accountService.GeneratePasswordResetToken(user);
        //    var resetLink = Url.Action(nameof(ResetPassword), "Account", new { userId = user.Id, token }, Request.Scheme);

        //    var success = await _accountService.InitiatePasswordReset(forgetPasswordDto.Email, resetLink);

        //    if (!success)
        //    {
        //        return BadRequest("Could not initiate password reset.");
        //    }

        //    return Ok("Password reset link sent to your email.");
        //}

        // Reset the user's password using a token sent via email

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request.");
            }

            var result = await _accountService.ResetPassword(resetPasswordDto);

            if (result.Succeeded)
            {
                return Ok("Password reset successfully.");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.ToString());
            }

            return BadRequest(ModelState);
        }

        // Logout the user
        //[HttpPost("logout")]
        //[Authorize]
        //public async Task<IActionResult> Logout()
        //{
        //    await _accountService.Logout();
        //    return Ok("Logged out successfully.");
        //}
    }
}
