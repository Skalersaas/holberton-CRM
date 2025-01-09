using HolbertonCRM.Application.DTOs.Auth;
using HolbertonCRM.Application.Interfaces;
using HolbertonCRM.Models;
using HolbertonCRM.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        //private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        //private readonly IEmailService _emailService;

        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = GetUserByNameOrEmail(email).GetAwaiter().GetResult();
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
        public async Task<AppUser> GetUserByNameOrEmail(string userNameOrEmail)
        {
            return await _userManager.FindByNameAsync(userNameOrEmail) ?? await _userManager.FindByEmailAsync(userNameOrEmail);
        }
        public List<AppUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }

        public async Task<UserRegistrationResult> Register(RegisterDto registrationRequestDto)
        {
            AppUser user = CreateUserFromDto(registrationRequestDto);
            try
            {
                var result = _userManager.CreateAsync(user, registrationRequestDto.Password).GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    AssignRole(user.Email, UserRole.Staff.ToString());
                    SendVerificationEmailAsync(user);

                    return UserRegistrationResult.Success;
                }

                return UserRegistrationResult.Failed;

            }
            catch (Exception ex)
            {
                return UserRegistrationResult.Failed;
            }
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = GetUserByNameOrEmail(loginRequestDto.UserName).GetAwaiter().GetResult();

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            //if user was found , Generate JWT Token
            var roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDto userDTO = new()
            {
                //Email = user.Email,
                ID = user.Id,
                Name = user.UserName,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDTO,
                Token = token
            };

            return loginResponseDto;
        }

        //public async Task<bool> ConfirmEmailAndSignIn(ConfirmAccountDto confirmAccountDto)
        //{
        //    AppUser existUser = await _userManager.FindByEmailAsync(confirmAccountDto.Email);
        //    if (existUser == null)
        //    {
        //        return false;
        //    }

        //    if (existUser.OTP != confirmAccountDto.OTP || string.IsNullOrEmpty(confirmAccountDto.OTP))
        //    {
        //        return false;
        //    }

        //    string token = await _userManager.GenerateEmailConfirmationTokenAsync(existUser);
        //    await _userManager.ConfirmEmailAsync(existUser, token);
        //    await _signInManager.SignInAsync(existUser, isPersistent: false);

        //    return true;
        //}

        //public async Task<bool> ResendOTP(string email)
        //{
        //    string otp = GenerateOTP();
        //    AppUser existUser = await _userManager.FindByEmailAsync(email);

        //    if (existUser == null)
        //    {
        //        return false; // User not found, could not resend OTP.
        //    }

        //    existUser.OTP = otp;
        //    await _userManager.UpdateAsync(existUser);

        //    SendVerificationEmailAsync(existUser);
        //    return true;
        //}

        public async Task<IdentityResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            AppUser existUser = await _userManager.FindByNameAsync(changePasswordDto.UserName);
            if (existUser == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(existUser, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            return result;
        }

        public async Task<bool> InitiatePasswordReset(string email, string resetLink)
        {
            AppUser existUser = await _userManager.FindByEmailAsync(email);
            if (existUser == null)
            {
                return false;
            }

            _emailService.PrepareEmail(new EmailMember() { subject = "resetPassword", email = email, link = resetLink });
            return true;
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            AppUser existUser = await _userManager.FindByIdAsync(resetPasswordDto.UserId);
            if (existUser == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            if (await _userManager.CheckPasswordAsync(existUser, resetPasswordDto.Password))
            {
                return IdentityResult.Failed(new IdentityError { Description = "The new password must be different from the existing password." });
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(existUser, resetPasswordDto.Token, resetPasswordDto.Password);
            return result;
        }

        public async Task<string> GeneratePasswordResetToken(AppUser existUser)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(existUser);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }


        private AppUser CreateUserFromDto(RegisterDto registrationRequestDto)
        {
            if (registrationRequestDto != null)
            {
                var user = new AppUser()
                {
                    UserName = registrationRequestDto.Email,
                    Email = registrationRequestDto.Email,
                    NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                    FullName = registrationRequestDto.Name
                };
                return user;
            }
            return null;
        }


        //private void SendVerificationEmailAsync(AppUser appUser)
        //{
        //    _emailService.PrepareEmail(
        //        new EmailMember()
        //        {
        //            email = appUser.Email,
        //            subject = "verify",
        //            OTP = appUser.OTP
        //        }
        //    );
        //}
        //private static string GenerateOTP()
        //{
        //    Random random = new();
        //    int otpNumber = random.Next(1000, 9999);
        //    return otpNumber.ToString();
        //}
    }
}
