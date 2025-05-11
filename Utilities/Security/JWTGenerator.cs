using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Utilities.Security
{
    /// <summary>
    /// Provides functionality for generating JSON Web Tokens (JWT) for authentication.
    /// This class handles the creation of JWT tokens with standard claims and security configurations.
    /// </summary>
    public static class JwtTokenGenerator
    {
        public static readonly string SecretKey =
            Environment.GetEnvironmentVariable("SecretKey") ?? throw new InvalidOperationException("SecretKey is not defined");
        public const string Issuer = "King";
        public const string Audience = "Soldiers";
        public const int LifetimeSeconds = 3600;

        private static readonly byte[] KeyBytes = Encoding.UTF8.GetBytes(SecretKey);
        private static readonly SymmetricSecurityKey SecurityKey = new(KeyBytes);
        private static readonly SigningCredentials Credentials = new(SecurityKey, SecurityAlgorithms.HmacSha256);
       
        /// <summary>
        /// Generates a JWT token for the specified username.
        /// </summary>
        /// <param name="username">The username to include in the token claims.</param>
        /// <returns>A signed JWT token as a string.</returns>
        /// <remarks>
        /// Creates a token containing username and JTI claims, with a 1-hour expiration time and standard issuer/audience claims.
        /// </remarks>
        public static string GenerateJwtToken(string username)
        {
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims:
                [
                    new Claim(ClaimTypes.Name, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                ],
                expires: DateTime.UtcNow.AddSeconds(LifetimeSeconds),
                signingCredentials: Credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
