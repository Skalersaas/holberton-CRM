using HolbertonCRM.Application.DTOs;
using HolbertonCRM.Application.DTOs.Auth;
using HolbertonCRM.Application.Interfaces;
using HolbertonCRM.Utilities.ConstantMessages;
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
        private readonly ResponseDto _responseDto;
        private readonly IAuthService _accountService;

        public AuthController(IAuthService accountService)
        {
            _accountService = accountService;
            _responseDto = new ResponseDto();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = AuthMessages.InvalidModelState;
                return BadRequest(_responseDto);
            }

            var registrationResult = await _accountService.Register(registerDto);

            if (registrationResult == UserRegistrationResult.Success)
            {
                _responseDto.Message = AuthMessages.RegistrationSuccessful;
            }

            _responseDto.IsSuccess = false;
            _responseDto.Message = AuthMessages.RegistrationFailed;
            return BadRequest(_responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = AuthMessages.InvalidLoginAttempt;
                return BadRequest(_responseDto);
            }

            var loginResult = await _accountService.Login(loginDto);

            if (loginResult.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = AuthMessages.InvalidUsernameOrPassword;
                return Unauthorized(_responseDto);
            }

            _responseDto.Result = new { loginResult.User, loginResult.Token };
            _responseDto.Message = AuthMessages.LoginSuccessful;
            return Ok(_responseDto);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = AuthMessages.InvalidPasswordChangeRequest;
                return BadRequest(_responseDto);
            }

            var result = await _accountService.ChangePassword(changePasswordDto);

            if (result.Succeeded)
            {
                _responseDto.Message = AuthMessages.PasswordChangeSuccessful;
                return Ok(_responseDto);
            }

            _responseDto.IsSuccess = false;
            _responseDto.Message = AuthMessages.PasswordChangeFailed;
            _responseDto.Result = result.Errors;
            return BadRequest(_responseDto);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordDto forgetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request.");
            }

            var user = await _accountService.GetUserByNameOrEmail(forgetPasswordDto.Email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var token = await _accountService.GeneratePasswordResetToken(user);
            var resetLink = Url.Action(nameof(ResetPassword), "Account", new { userId = user.Id, token }, Request.Scheme);

            var success = await _accountService.InitiatePasswordReset(forgetPasswordDto.Email, resetLink);

            if (!success)
            {
                return BadRequest("Could not initiate password reset.");
            }

            return Ok("Password reset link sent to your email.");
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = AuthMessages.InvalidPasswordResetRequest;
                return BadRequest(_responseDto);
            }

            var result = await _accountService.ResetPassword(resetPasswordDto);

            if (result.Succeeded)
            {
                _responseDto.Message = AuthMessages.PasswordResetSuccessful;
                return Ok(_responseDto);
            }

            _responseDto.IsSuccess = false;
            _responseDto.Message = AuthMessages.PasswordResetFailed;
            _responseDto.Result = result.Errors;
            return BadRequest(_responseDto);
        }

       [HttpPost("logout")]
       [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return Ok("Logged out successfully.");
        }
    }
}
