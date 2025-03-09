using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Utilities.Services
{

    public class JwtTokenGenerator
    {
        public static readonly string _secretKey = 
            Environment.GetEnvironmentVariable("SecretKey") ?? throw new Exception("SecretKey is not defined");
        public static readonly string _issuer = "King";
        public static readonly string _audience = "Soldiers";
        public static readonly int _lifeTime = 3600;
        public static string GenerateJwtToken(string username)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, username),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddSeconds(_lifeTime),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
