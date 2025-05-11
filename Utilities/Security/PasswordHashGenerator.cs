using System.Security.Cryptography;
using System.Text;

namespace Utilities.Security
{
    /// <summary>
    /// Provides functionality for secure password hashing and verification.
    /// This class implements a secure password hashing scheme using salt and SHA-256.
    /// </summary>
    public static class PasswordHashGenerator
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;

        /// <summary>
        /// Generates a secure hash for a given password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A base64-encoded string containing both the salt and hash.</returns>
        /// <remarks>
        /// Creates a cryptographically secure hash by combining a random salt with the password and applying SHA-256 hashing.
        /// </remarks>
        public static string GenerateHash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = HashPassword(password, salt);

            var hashBytes = new byte[SaltSize + HashSize];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Verifies if an entered password matches a stored hash.
        /// </summary>
        /// <param name="storedHash">The stored hash to verify against.</param>
        /// <param name="enteredPassword">The password to verify.</param>
        /// <returns>True if the password matches the stored hash, false otherwise.</returns>
        /// <remarks>
        /// Extracts the salt from the stored hash, hashes the entered password with the same salt, and performs a constant-time comparison.
        /// </remarks>
        public static bool VerifyPassword(string storedHash, string enteredPassword)
        {
            var hashBytes = Convert.FromBase64String(storedHash);
            var salt = new byte[SaltSize];
            var storedHashBytes = new byte[HashSize];

            Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);
            Buffer.BlockCopy(hashBytes, SaltSize, storedHashBytes, 0, HashSize);

            var enteredHash = HashPassword(enteredPassword, salt);
            return CryptographicOperations.FixedTimeEquals(storedHashBytes, enteredHash);
        }

        /// <summary>
        /// Hashes a password with a given salt using SHA-256.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt to use for hashing.</param>
        /// <returns>The resulting hash as a byte array.</returns>
        /// <remarks>
        /// Combines the password and salt bytes, then applies SHA-256 hashing to the combined data.
        /// </remarks>
        private static byte[] HashPassword(string password, byte[] salt)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var salted = new byte[passwordBytes.Length + salt.Length];

            Buffer.BlockCopy(passwordBytes, 0, salted, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, salted, passwordBytes.Length, salt.Length);

            return SHA256.HashData(salted);
        }
    }
}
