using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities.Services
{
    public class PasswordHashGenerator
    {
        private const int SaltSize = 16; 
        private const int HashSize = 32;

        public static string GenerateHash(string password)
        {
            byte[] salt = GenerateSalt();

            byte[] hash = HashPassword(password, salt);

            byte[] hashBytes = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, hashBytes, 0, salt.Length);
            Array.Copy(hash, 0, hashBytes, salt.Length, hash.Length);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string storedHash, string enteredPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            byte[] storedPasswordHash = new byte[HashSize];
            Array.Copy(hashBytes, SaltSize, storedPasswordHash, 0, HashSize);

            byte[] enteredPasswordHash = HashPassword(enteredPassword, salt);

            return CompareHashes(storedPasswordHash, enteredPasswordHash);
        }

        private static byte[] HashPassword(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];
            Array.Copy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
            Array.Copy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

            return SHA256.HashData(saltedPassword);
        }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[SaltSize];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        private static bool CompareHashes(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
            {
                return false;
            }

            int diff = 0;
            for (int i = 0; i < hash1.Length; i++)
            {
                diff |= hash1[i] ^ hash2[i];
            }

            return diff == 0;
        }
    }
}