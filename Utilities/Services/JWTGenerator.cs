using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Utilities.Services
{

    public class JwtTokenGenerator
    {
        public static readonly string SecretKey =
            Environment.GetEnvironmentVariable("SecretKey") ?? throw new Exception("SecretKey is not defined");
        public static readonly string Issuer = "King";
        public static readonly string Audience = "Soldiers";
        public static readonly int LifeTime = 3600;
        
        public static string GenerateJwtToken(string username)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, username),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.Now.AddSeconds(LifeTime),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
