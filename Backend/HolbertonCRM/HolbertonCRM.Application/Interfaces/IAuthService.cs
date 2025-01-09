using HolbertonCRM.Application.DTOs.Auth;
using HolbertonCRM.Models;
using HolbertonCRM.Utilities.Enums;
using Microsoft.AspNetCore.Identity;


namespace HolbertonCRM.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserRegistrationResult> Register(RegisterDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        //Task<bool> ConfirmEmailAndSignIn(ConfirmAccountDto confirmAccountDto);
        //Task<bool> ResendOTP(string email);
        Task<IdentityResult> ChangePassword(ChangePasswordDto changePasswordDto);
        //Task<bool> InitiatePasswordReset(string email, string resetLink);
        Task<IdentityResult> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<AppUser> GetUserByNameOrEmail(string userNameOrEmail);
        //Task<string> GeneratePasswordResetToken(AppUser existUser);
        //Task<List<AppUser>> GetAllUsers();
        Task<bool> AssignRole(string email, string roleName);
        //Task Logout();
    }
}
