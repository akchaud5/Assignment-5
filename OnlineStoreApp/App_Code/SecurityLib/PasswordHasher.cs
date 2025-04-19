using System;
using System.Security.Cryptography;
using System.Text;

namespace SecurityLib
{
    public class PasswordHasher
    {
        // Simple password hashing implementation
        public static string HashPassword(string password)
        {
            // Use SHA256 for hashing
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Verify a password against a hash
        public static bool VerifyPassword(string password, string hash)
        {
            string hashOfInput = HashPassword(password);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}