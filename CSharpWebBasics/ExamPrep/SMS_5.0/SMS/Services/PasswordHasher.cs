using System;
using System.Security.Cryptography;
using System.Text;

namespace SMS.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return string.Empty;
            }

            // ComputeHash - returns byte array  
            using var sha256Hash = SHA256.Create();

            // Convert byte array to a string
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            var sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
