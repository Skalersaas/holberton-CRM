using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Utilities.Services
{
    internal class JWTGenerator
    {
    }
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "mysupersecret_secretsecretsecretkey!123";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new(Encoding.UTF8.GetBytes(KEY));
        public static string GetJWTToken(string userLogin, int userId)
        {
            var claims = new List<Claim>() {
                new("Id", userId.ToString()),
                new("Login", userLogin),
            };
            var jwt = new JwtSecurityToken(
                issuer: ISSUER,
                audience: AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(1)),
                signingCredentials: new(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
