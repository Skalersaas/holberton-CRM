
using Microsoft.AspNetCore.Identity;

namespace HolbertonCRM.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public string? OTP { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
